using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Users;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Users
{
    public class GetUserHandler : IRequestHandler<GetUser, User>
    {
        private readonly IDbConnection _DbConnection;

        public GetUserHandler(IDbConnection dbConnection)
        {
            _DbConnection = dbConnection;
        }

        public User Handle(GetUser message)
        {
            const string query = 
                "SELECT Id, FirstName, LastName, Email FROM Users.[User] u " +
                "WHERE u.Id = @UserId";

            var users = _DbConnection.Query<User>(query, message);

            return users.FirstOrDefault();
        }
    }
}
