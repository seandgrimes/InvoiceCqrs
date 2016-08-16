using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using Dapper;
using InvoiceCqrs.Domain.Entities.EventStore;
using InvoiceCqrs.Messages.Commands.EventStore;
using InvoiceCqrs.Persistence;
using InvoiceCqrs.Util;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.EventStore
{
    public class AddEventHandler : IRequestHandler<AddEvent, Domain.Entities.EventStore.Event>
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;

        public AddEventHandler(IUnitOfWork unitOfWork, IGuidGenerator guidGenerator)
        {
            _UnitOfWork = unitOfWork;
            
            _Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddEvent, Domain.Entities.EventStore.Event>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EventId));

                cfg.CreateMap<IDictionary<string, string>, IList<EventMetadata>>()
                    .ConstructUsing(
                        dict => dict.Keys.Select(key => new EventMetadata()
                        {
                            Name = key,
                            Value = dict[key],
                            Id = guidGenerator.Generate()
                        }).ToList());
            }).CreateMapper();
        }

        public Domain.Entities.EventStore.Event Handle(AddEvent message)
        {
            var entity = _Mapper.Map<Domain.Entities.EventStore.Event>(message);

            const string eventQuery =
                @"  INSERT INTO EventStore.Event (Id, CorrelationId, EventDate, EventType, IsDispatched, Json, SourceEventId, StreamId)
                VALUES (@Id, @CorrelationId, @EventDate, @EventType, @IsDispatched, @Json, @SourceEventId, @StreamId)";

            _UnitOfWork.Execute(eventQuery, entity);

            const string metadataQuery =
                @"  INSERT INTO EventStore.EventMetadata (Id, EventId, Name, Value)
                    VALUES (@Id, @EventId, @Name, @Value)";

            var metadata = _Mapper.Map<IList<EventMetadata>>(message.Metadata)
                .Select(meta => new
                {
                    meta.Id,
                    EventId = entity.Id,
                    meta.Name,
                    meta.Value
                });

            _UnitOfWork.Execute(metadataQuery, metadata);

            return entity;
        }
    }
}
