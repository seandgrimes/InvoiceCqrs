using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Messages.Queries.Users;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Users
{
    public class GetUserHandler : IRequestHandler<GetUser, User>
    {
        public User Handle(GetUser message)
        {
            return ReadModel.Users[message.UserId];
        }
    }
}
