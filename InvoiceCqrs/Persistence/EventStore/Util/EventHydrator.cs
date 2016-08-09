using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceCqrs.Messages.Events;
using Newtonsoft.Json;

namespace InvoiceCqrs.Persistence.EventStore.Util
{
    public class EventHydrator : IEventHydrator
    {
        public IList<IEvent> Hydrate(IList<Domain.Entities.EventStore.Event> events)
        {
            var hydrated = events
                .Select(evt => (IEvent) JsonConvert.DeserializeObject(evt.Json, Type.GetType(evt.EventType)))
                .ToList();

            return hydrated;
        }
    }
}
