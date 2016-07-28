using System;

namespace InvoiceCqrs.Persistence.EventStore
{
    public class Event
    {
        public Type EventType { get; set; }

        public Guid ExternalId { get; set; }

        public string JsonContent { get; set; }

        
    }
}
