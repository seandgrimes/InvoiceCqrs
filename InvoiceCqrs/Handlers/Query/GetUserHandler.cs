using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query
{
    public class GetUserHandler : IRequestHandler<GetUser, User>
    {
        public User Handle(GetUser message)
        {
            return ReadModel.Users[message.UserId];
        }
    }
}
