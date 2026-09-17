using NotificationDeliveryServices.Contracts;
using NotificationDeliveryServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Providers
{
    public interface INotificationProvider
    {
        NotificationChannelEnum Channel { get; }

        Task SendAsync(NotificationCreatedEvent notification, CancellationToken cancellationToken);
    }
}
