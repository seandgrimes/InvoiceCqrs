using System.Data;
using AutoMapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands.Users;
using InvoiceCqrs.Messages.Events.Users;
using InvoiceCqrs.Messages.Queries.Users;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Users
{
    public class CreateUserHandler : IRequestHandler<CreateUser, User>
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public CreateUserHandler(Store store, IMediator mediator)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Users);

            _Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateUser, UserCreated>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            }).CreateMapper();
        }

        public User Handle(CreateUser message)
        {
            _Stream.Write<UserCreated>(builder => builder
                .WithCorrelationId(message.Id)
                .WithEvent(_Mapper.Map<UserCreated>(message)));

            return _Mediator.Send(new GetUser {UserId = message.Id});
        }
    }
}