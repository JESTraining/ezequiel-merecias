using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace APIProject.Infraestructure.Messaging
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly RabbitMqOptions options;

        public RabbitMqEventPublisher(IOptions<RabbitMqOptions> _options)
        {
            options = _options.Value;
        }

        public async Task PublishAsync(string eventType, string payload, CancellationToken cancellationToken = default)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = options.HostName,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password
            };

            await using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

            //Create Communication Channel to connecting RabbitMQ and enable exception when fail
            await using var channel = await connection.CreateChannelAsync(new CreateChannelOptions(true, true),
                cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(options.ExchangeName, ExchangeType.Topic, true, false, cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(payload);

            //publish the message
            await channel.BasicPublishAsync(options.ExchangeName, eventType, body, cancellationToken);
        }
    }
}
