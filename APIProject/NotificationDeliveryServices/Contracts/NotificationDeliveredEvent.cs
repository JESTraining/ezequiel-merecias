using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Contracts
{
    public record NotificationDeliveredEvent(Guid MessageId, Guid NotificationId, DateTime DeliveredDate);
}
