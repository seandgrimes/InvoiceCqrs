using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using InvoiceCqrs.Persistence.EventStore.Util;
using MediatR;
using Newtonsoft.Json;

namespace InvoiceCqrs.Persistence.EventStore
{
    public class Stream
    {
        private readonly IMediator _Mediator;
        public IList<Event> Events { get; set; } = new List<Event>();

        public string Name { get; private set; }

        public Stream(string streamName, IMediator mediator)
        {
            _Mediator = mediator;
            Name = streamName;
        }

        [Obsolete("Use the fluent builder overload of Stream.Write instead")]
        public TEvent Write<TEvent>(Guid externalId, TEvent evt) where TEvent : INotification
        {
            Events.Add(new Event
            {
                ExternalId = externalId,
                JsonContent = JsonConvert.SerializeObject(evt),
                EventType = evt.GetType()
            });

            _Mediator.Publish(evt);

            return evt;
        }

        public TEvent Write<TEvent>(Expression<Action<EventBuilder<TEvent>>> buildExpr) where TEvent : INotification
        {
            var builder = new EventBuilder<TEvent>();
            var action = buildExpr.Compile();

            action(builder);

            var evt = builder.Build();
            Events.Add(evt);

            _Mediator.Publish(builder.Event);

            return builder.Event;
        }
    }
}