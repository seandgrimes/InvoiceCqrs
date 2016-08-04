using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using InvoiceCqrs.Messages.Commands.EventStore;
using InvoiceCqrs.Persistence.EventStore.Util;
using InvoiceCqrs.Util;
using MediatR;

namespace InvoiceCqrs.Persistence.EventStore
{
    public class Stream
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly IGuidGenerator _GuidGenerator;
        private readonly Guid _StreamId;


        public IList<Event> Events { get; set; } = new List<Event>();

        public Stream(Guid streamId, IMediator mediator, IGuidGenerator guidGenerator)
        {
            _StreamId = streamId;
            _Mediator = mediator;
            _GuidGenerator = guidGenerator;

            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Event, AddEvent>()
                    .ForMember(dest => dest.CorrelationId, opt => opt.MapFrom(src => src.ExternalId))
                    .ForMember(dest => dest.Json, opt => opt.MapFrom(src => src.JsonContent));
            });

            _Mapper = mapperCfg.CreateMapper();
        }

        public TEvent Write<TEvent>(Expression<Action<EventBuilder<TEvent>>> buildExpr) where TEvent : INotification
        {
            var builder = new EventBuilder<TEvent>();
            var action = buildExpr.Compile();

            action(builder);

            var evt = builder.Build();
            evt.EventDate = DateTime.UtcNow;
            evt.EventId = _GuidGenerator.Generate();
            evt.StreamId = _StreamId;

            Events.Add(evt);
            PersistEvent(evt);

            _Mediator.Publish(builder.Event);

            return builder.Event;
        }

        private void PersistEvent(Event evt)
        {
            var eventEntity = _Mapper.Map<AddEvent>(evt);
            _Mediator.Send(eventEntity);
        }
    }
}