using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Contracts
{
    public record NotificationFailedEvent(Guid MessageId, Guid NotificationId, string error, DateTime FailedDate);
}
