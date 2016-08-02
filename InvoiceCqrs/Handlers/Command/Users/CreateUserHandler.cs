using System.Data;
using Dapper;
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
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public CreateUserHandler(Store store, IMediator mediator)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Users);
        }

        public User Handle(CreateUser message)
        {
            _Stream.Write(message.Id, new UserCreated
            {
                Email = message.Email,
                FirstName = message.FirstName,
                LastName = message.LastName,
                UserId = message.Id
            });

            return _Mediator.Send(new GetUser {UserId = message.Id});
        }
    }
}

public static class DbCommandExtensions
{
    public static void AddParameterWithValue(this IDbCommand command, string paramName, object value)
    {
        var param = command.CreateParameter();
        param.ParameterName = paramName;
        param.Value = value;

        command.Parameters.Add(param);
    }
}
