using System;

namespace InvoiceCqrs.Domain.Entities.EventStore
{
    public class EventMetadata : Entity
    {
        public Event Event { get; set; }

        public Guid EventId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
