using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Newtonsoft.Json;

namespace InvoiceCqrs.EventStore
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
    }
}