using System;
using System.Collections.Generic;
using InvoiceCqrs.Messages.Events;

namespace InvoiceCqrs.Persistence.EventStore
{
    public class EventWrapper
    {
        public DateTime EventDate { get; set; }

        public Type EventType { get; set; }

        public Guid EventId { get; set; }

        public Guid ExternalId { get; set; }

        public string JsonContent { get; set; }

        public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        public Guid? SourceEventId { get; set; }

        public Guid StreamId { get; set; }
    }

    public class EventWrapper<TEvent> : EventWrapper where TEvent : IEvent
    {
        public TEvent Event { get; set; }
    }
}
