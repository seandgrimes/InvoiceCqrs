using System;
using System.Collections.Generic;

namespace InvoiceCqrs.Persistence.EventStore
{
    public class Event
    {
        public DateTime EventDate { get; set; }

        public Type EventType { get; set; }

        public Guid EventId { get; set; }

        public Guid ExternalId { get; set; }

        public string JsonContent { get; set; }

        public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        public Guid StreamId { get; set; }
    }
}
