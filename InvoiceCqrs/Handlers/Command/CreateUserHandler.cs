using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands;
using InvoiceCqrs.Messages.Events.Users;
using InvoiceCqrs.Persistence;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command
{
    public class CreateUserHandler : IRequestHandler<CreateUser, User>
    {
        private readonly Stream _Stream;

        public CreateUserHandler(Store store)
        {
            _Stream = store.Open(Streams.Users);
        }

        public User Handle(CreateUser message)
        {
            var user = new User
            {
                Email = message.Email,
                FirstName = message.FirstName,
                Id = message.Id,
                LastName = message.LastName
            };
            ReadModel.Users[user.Id] = user;

            _Stream.Write(user.Id, new UserCreated
            {
                UserId = user.Id
            });

            return ReadModel.Users[message.Id];
        }
    }
}
