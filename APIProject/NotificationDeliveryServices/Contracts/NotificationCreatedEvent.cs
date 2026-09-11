using NotificationDeliveryServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Contracts
{
    public record NotificationCreatedEvent(Guid MessageId, Guid NotificationId, Guid UserId,string Subject, string Content, NotificationChannelEnum Channel, NotificationPriorityEnum Priority, DateTime CreatedDate);
}
