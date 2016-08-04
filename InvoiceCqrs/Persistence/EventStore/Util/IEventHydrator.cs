using System.Collections.Generic;
using InvoiceCqrs.Messages.Events;

namespace InvoiceCqrs.Persistence.EventStore.Util
{
    public interface IEventHydrator
    {
        IList<IEvent> Hydrate(IList<Domain.Entities.EventStore.Event> events);
    }
}