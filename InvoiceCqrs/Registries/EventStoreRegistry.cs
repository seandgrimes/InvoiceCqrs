using InvoiceCqrs.Persistence.EventStore;
using InvoiceCqrs.Persistence.EventStore.Util;
using StructureMap;

namespace InvoiceCqrs.Registries
{
    public class EventStoreRegistry : Registry
    {
        public EventStoreRegistry()
        {
            For<Store>().Use<Store>();
            For<IEventHydrator>().Use<EventHydrator>();
        }
    }
}
