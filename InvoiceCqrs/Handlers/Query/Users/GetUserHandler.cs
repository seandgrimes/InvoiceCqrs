using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Users;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Users
{
    public class GetUserHandler : IRequestHandler<GetUser, User>
    {
        private readonly IUnitOfWork _UnitOfWork;
        
        public GetUserHandler(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public User Handle(GetUser message)
        {
            const string query = 
                "SELECT Id, FirstName, LastName, Email FROM Users.[User] u " +
                "WHERE u.Id = @UserId";

            var users = _UnitOfWork.Query<User>(query, message);

            return users.FirstOrDefault();
        }
    }
}
