using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace InvoiceCqrs.Persistence.EventStore.Util
{
    public class EventBuilder<TEvent>
    {
        public Guid CorrelationId { get; private set; }
        public TEvent Event { get; private set; }
        public IDictionary<string, string> Metadata { get; private set; }

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

        public Event Build()
        {
            if (Event == null)
            {
                throw new InvalidOperationException("The WithEvent method must be called with a non-null value");
            }

            return new Event
            {
                EventType = Event.GetType(),
                ExternalId = CorrelationId,
                JsonContent = JsonConvert.SerializeObject(Event),
                Metadata = Metadata
            };
        }
    }
}
