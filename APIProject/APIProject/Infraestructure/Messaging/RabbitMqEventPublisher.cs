using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace APIProject.Infraestructure.Messaging
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly RabbitMqOptions _options;

        public RabbitMqEventPublisher(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        public async Task PublishAsync(string eventType, string payload, CancellationToken cancellationToken = default)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password
            };

            await using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

            //Create Communication Channel to connecting RabbitMQ and enable exception when fail
            await using var channel = await connection.CreateChannelAsync(new CreateChannelOptions(true, true),
                cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(_options.ExchangeName, ExchangeType.Topic, true, false, cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(payload);

            //publish the message
            await channel.BasicPublishAsync(_options.ExchangeName, eventType, body, cancellationToken);
        }
    }
}
