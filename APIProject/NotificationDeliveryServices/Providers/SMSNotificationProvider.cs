using NotificationDeliveryServices.Contracts;
using NotificationDeliveryServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Providers
{
    public class SMSNotificationProvider : INotificationProvider
    {
        public NotificationChannelEnum Channel => NotificationChannelEnum.Sms;
        public readonly ILogger<SMSNotificationProvider> logger;

        public SMSNotificationProvider(ILogger<SMSNotificationProvider> _logger)
        {
            logger = _logger;
        }

        public async Task SendAsync(NotificationCreatedEvent notification, CancellationToken cancellationToken)
        {
            await Task.Delay(500, cancellationToken);

            logger.LogInformation("SMS sent notification:{NotificationId}", notification.NotificationId);
        }
    }
}
