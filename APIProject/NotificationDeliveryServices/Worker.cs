using NotificationDeliveryServices.Messaging;
using NotificationDeliveryServices.Providers;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using NotificationDeliveryServices.Contracts;
using Microsoft.Extensions.Options;

namespace NotificationDeliveryServices
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly RabbitMqOptions options;
        private readonly NotificationProviderResolver providerResolver;

        public Worker(ILogger<Worker> _logger, NotificationProviderResolver _providerResolver, IOptions<RabbitMqOptions> _options)
        {
            logger = _logger;
            providerResolver = _providerResolver;
            options = _options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = options.HostName,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password
            };

            await using var connection = await factory.CreateConnectionAsync(stoppingToken);

            await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            //Main Exchange Headache :/
            await channel.ExchangeDeclareAsync(exchange: options.ExchangeName, type: ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: stoppingToken);

            //Dead Letter Exchange
            await channel.ExchangeDeclareAsync(exchange: options.DeadLetterExchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, cancellationToken: stoppingToken);

            //Dead Letter Queue
            await channel.QueueDeclareAsync(queue: options.DeadLetterQueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

            //Dead Letter Binding
            await channel.QueueBindAsync(queue: options.DeadLetterQueueName, exchange: options.DeadLetterExchangeName, routingKey: "notification.failed", cancellationToken: stoppingToken);

            //Create Queue when failed
            var queue = new Dictionary<string, object?>
            {
                ["x-dead-letter-exchange"] = options.DeadLetterExchangeName,
                ["x-dead-letter-routing-key"] = "notification.failed"
            };

            //Create Delivery on RabbitMQ with durable queue
            await channel.QueueDeclareAsync(queue: options.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: queue, cancellationToken: stoppingToken);

            //Connection between our Exchange and Queue, catch the copy when is created
            await channel.QueueBindAsync(queue: options.QueueName, exchange: options.ExchangeName, routingKey: "notification.created", cancellationToken: stoppingToken);

            //Set Limit to 10 messages to process at the same time
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, eventArgs) =>
            {
                await HandleMessageAsync(channel, eventArgs, stoppingToken);
            };

            //Dont delete the message until ACK flag catch
            await channel.BasicConsumeAsync(queue: options.QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            logger.LogInformation("Waiting for messages...");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task HandleMessageAsync(IChannel channel, BasicDeliverEventArgs eventArgs, CancellationToken cancellationToken)
        {
            try
            {
                var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                var notification = JsonSerializer.Deserialize<NotificationCreatedEvent>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (notification is null)
                {
                    throw new Exception("Error trying to deserialize notification message.");
                }

                logger.LogInformation("Notification received: {NotificationId}", notification.NotificationId);

                var provider = providerResolver.Resolve(notification.Channel);

                await provider.SendAsync(notification, cancellationToken);

                await channel.BasicAckAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false, cancellationToken: cancellationToken);

                logger.LogInformation("Notification {NotificationId} processed successfully", notification.NotificationId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing notification message");

                await channel.BasicNackAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: false, cancellationToken: cancellationToken);
            }
        }
    }
}
