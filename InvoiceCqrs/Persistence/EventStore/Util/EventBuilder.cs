using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using InvoiceCqrs.Messages.Events;
using Newtonsoft.Json;

namespace InvoiceCqrs.Persistence.EventStore.Util
{
    public class EventBuilder<TEvent> where TEvent : IEvent
    {
        public Guid CorrelationId { get; private set; }
        public TEvent Event { get; private set; }
        public IDictionary<string, string> Metadata { get; private set; }
        public Guid? SourceEventId { get; set; }

        public EventBuilder<TEvent> WithCorrelationId(Guid correlationId)
        {
            CorrelationId = correlationId;

            return this;
        }

        public EventBuilder<TEvent> WithEvent(TEvent evt)
        {
            Event = evt;

            return this;
        }

        public EventBuilder<TEvent> WithMetaData(Expression<Func<TEvent,object>> metadataExpr)
        {
            if (Event == null)
            {
                throw new InvalidOperationException("The WithEvent method must be called before the WithMetaData method");
            }

            var compiled = metadataExpr.Compile();
            var metadata = compiled(Event);

            var mapper = new ObjectToPropertyDictionaryMapper();
            Metadata = mapper.Map(metadata);

            return this;
        }

        public EventBuilder<TEvent> WithSourceEventId(Guid sourceEventId)
        {
            SourceEventId = sourceEventId;

            return this;
        }

        public EventWrapper<TEvent> Build()
        {
            if (Event == null)
            {
                throw new InvalidOperationException("The WithEvent method must be called with a non-null value");
            }

            var wrapper = new EventWrapper<TEvent>
            {
                Event = Event,
                EventType = Event.GetType(),
                ExternalId = CorrelationId,
                JsonContent = JsonConvert.SerializeObject(Event),
                Metadata = Metadata,
                SourceEventId = SourceEventId
            };

            return wrapper;
        }
    }
}
