using System;
using System.Collections.Generic;
using InvoiceCqrs.Domain.Entities.EventStore;
using MediatR;

namespace InvoiceCqrs.Messages.Commands.EventStore
{
    public class AddEvent : IRequest<Event>
    {
        public Guid CorrelationId { get; set; }

        public DateTime EventDate { get; set; }

        public Guid EventId { get; set; }

        public string EventType { get; set; }

        public bool IsDispatched { get; set; }

        public string Json { get; set; }

        public IDictionary<string, string> Metadata { get; set; }

        public Guid? SourceEventId { get; set; }

        public Guid StreamId { get; set; } 
    }
}
