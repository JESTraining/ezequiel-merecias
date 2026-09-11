using NotificationDeliveryServices.Contracts;
using NotificationDeliveryServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Providers
{
    public class PushNotificationProvider : INotificationProvider
    {
        public NotificationChannelEnum Channel => NotificationChannelEnum.Push;

        public readonly ILogger<PushNotificationProvider> logger;

        public PushNotificationProvider(ILogger<PushNotificationProvider> _logger)
        {
            logger = _logger;
        }

        public async Task SendAsync(NotificationCreatedEvent notification, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);

            logger.LogInformation("Push sent Id:{NotificationId}", notification.NotificationId);
        }
    }
}
