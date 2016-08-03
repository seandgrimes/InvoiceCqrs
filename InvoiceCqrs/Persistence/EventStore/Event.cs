using System;
using System.Collections.Generic;

namespace InvoiceCqrs.Persistence.EventStore
{
    public class Event
    {
        public Type EventType { get; set; }

        public Guid ExternalId { get; set; }

        public string JsonContent { get; set; }

        public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}
