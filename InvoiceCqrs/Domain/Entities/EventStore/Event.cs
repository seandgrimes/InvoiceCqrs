using System;
using System.Collections.Generic;

namespace InvoiceCqrs.Domain.Entities.EventStore
{
    public class Event : Entity
    {
        public Guid CorrelationId { get; set; }

        public DateTime EventDate { get; set; }

        public string EventType { get; set; }

        public bool IsDispatched { get; set; }

        public string Json { get; set; }

        public IDictionary<string, string> Metadata { get; set; }

        public Guid StreamId { get; set; }
    }
}
