using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Data.Entities
{
    public class ProcessedMessage
    {
        public Guid MessageId { get; set; }
        public DateTime ProcessedDate { get; set; }
    }
}
