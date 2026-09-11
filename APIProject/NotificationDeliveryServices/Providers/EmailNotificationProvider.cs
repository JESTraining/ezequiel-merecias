using NotificationDeliveryServices.Contracts;
using NotificationDeliveryServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Providers
{
    public class EmailNotificationProvider : INotificationProvider
    {
        public NotificationChannelEnum Channel => NotificationChannelEnum.Email;

        private readonly ILogger<EmailNotificationProvider> logger;

        public EmailNotificationProvider(ILogger<EmailNotificationProvider> _logger)
        {
            logger = _logger;
        }

        public async Task SendAsync(NotificationCreatedEvent notification, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);

            logger.LogInformation("Email sent Id:{NotificationId}", notification.NotificationId);
        }
    }
}
