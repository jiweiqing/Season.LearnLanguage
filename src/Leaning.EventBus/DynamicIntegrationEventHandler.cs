using Dynamic.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace Leaning.EventBus
{
    public abstract class DynamicIntegrationEventHandler: IIntegrationEventHandler
    {
        public Task Handle(string eventName, string eventData)
        {
            dynamic dynamicEventData = DJson.Parse(eventData);
            return HandleDynamic(eventName, dynamicEventData);
        }

        public abstract Task HandleDynamic(string eventName, dynamic eventData);
    }
}
