using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Messaging
{
    public class RabbitMqEventPublisher
    {
        private readonly RabbitMqOptions options;
        public RabbitMqEventPublisher(IOptions<RabbitMqOptions> _options)
        {
            options = _options.Value;
        }

        public async Task PublishAsync(string routingKey, string payload, CancellationToken cancellationToken)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = options.HostName,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password
            };

            await using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

            await using var channel = await connection.CreateChannelAsync(new CreateChannelOptions(true, true),
               cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(options.ExchangeName, ExchangeType.Topic, true, false, cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(payload);

            //publish the message
            await channel.BasicPublishAsync(options.ExchangeName, routingKey, body, cancellationToken);
        }
    }
}
