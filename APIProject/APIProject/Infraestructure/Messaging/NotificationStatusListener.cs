using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using APIProject.Infraestructure.Persistance;
using APIProject.Domain.Events;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace APIProject.Infraestructure.Messaging
{
    public class NotificationStatusListener : BackgroundService
    {
        private readonly ILogger<NotificationStatusListener> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly RabbitMqOptions options;
        private readonly IConnectionMultiplexer redis;

        public NotificationStatusListener(ILogger<NotificationStatusListener> _logger, IServiceScopeFactory _scopeFactory, IOptions<RabbitMqOptions> _options,
            IConnectionMultiplexer _redis)
        {
            logger = _logger;
            scopeFactory = _scopeFactory;
            options = _options.Value;
            redis = _redis;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = options.HostName,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password
            };

            await using var connection = await factory.CreateConnectionAsync(stoppingToken);

            await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            const string queueName = "notification.status";

            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

            await channel.QueueBindAsync(queue: queueName, exchange: options.ExchangeName, routingKey: "notification.delivered", cancellationToken: stoppingToken);

            await channel.QueueBindAsync(queue: queueName, exchange: options.ExchangeName, routingKey: "notification.failed", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, args) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(args.Body.ToArray());

                    using var scope = scopeFactory.CreateScope();

                    var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

                    if (args.RoutingKey == "notification.delivered")
                    {
                        var evt = JsonSerializer.Deserialize<NotificationDeliveredEvent>(json,new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });

                        if (evt != null)
                        {
                            var notification = await dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == evt.NotificationId, stoppingToken);

                            if (notification != null)
                            {
                                notification.MarkAsDelivered();

                                await dbContext.SaveChangesAsync(stoppingToken);

                                await redis.GetDatabase().KeyDeleteAsync($"notification-{notification.Id}"); //Delete from cache once delivered
                            }
                        }
                    }

                    if (args.RoutingKey == "notification.failed")
                    {
                        var evt = JsonSerializer.Deserialize<NotificationFailedEvent>(json,new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });

                        if (evt != null)
                        {
                            var notification = await dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == evt.NotificationId, stoppingToken);

                            if (notification != null)
                            {
                                notification.MarkAsFailed();

                                await dbContext.SaveChangesAsync(stoppingToken);

                                await redis.GetDatabase().KeyDeleteAsync($"notification-{notification.Id}"); //Delete from cache once failed
                            }
                        }
                    }

                    await channel.BasicAckAsync(args.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to process notification status event.");

                    await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
