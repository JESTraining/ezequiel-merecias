using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Messaging
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string ExchangeName { get; set; } = "notifications";
        public string QueueName { get; set; } = "delivery";
        public string DeadLetterExchangeName { get; set; } = "DLExchange";
        public string DeadLetterQueueName { get; set; } = "DLQueue";
    }
}
