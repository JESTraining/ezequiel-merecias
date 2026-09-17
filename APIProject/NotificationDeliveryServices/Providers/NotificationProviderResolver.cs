using NotificationDeliveryServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Providers
{
    public class NotificationProviderResolver
    {
        private readonly IEnumerable<INotificationProvider> providers;

        public NotificationProviderResolver(IEnumerable<INotificationProvider> _providers)
        {
            providers = _providers;
        }

        public INotificationProvider Resolve(NotificationChannelEnum channel)
        {
            var provider = providers.FirstOrDefault(x => x.Channel == channel);

            if(provider == null)
            {
                throw new Exception("Notification not found");
            }

            return provider;
        }
    }
}
