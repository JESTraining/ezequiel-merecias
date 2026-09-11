using NotificationDeliveryServices.Contracts;
using NotificationDeliveryServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Providers
{
    public class InAppNotificationProvider : INotificationProvider
    {
        public NotificationChannelEnum Channel => NotificationChannelEnum.InApp;

        public readonly ILogger<InAppNotificationProvider> logger;

        public InAppNotificationProvider(ILogger<InAppNotificationProvider> _logger)
        {
            logger = _logger;
        }

        public async Task SendAsync(NotificationCreatedEvent notification, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);

            logger.LogInformation("App Notification sent Id:{NotificationId}", notification.NotificationId);
        }
    }
}
