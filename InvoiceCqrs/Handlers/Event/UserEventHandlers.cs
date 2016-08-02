using System.Data;
using Dapper;
using InvoiceCqrs.Messages.Events.Users;
using MediatR;

namespace InvoiceCqrs.Handlers.Event
{
    public class UserEventHandlers : INotificationHandler<UserCreated>
    {
        private readonly IDbConnection _DbConnection;

        public UserEventHandlers(IDbConnection dbConnection)
        {
            _DbConnection = dbConnection;
        }

        public void Handle(UserCreated notification)
        {
            const string query =
                "INSERT INTO Users.[User] (Id, Email, FirstName, LastName, CreatedOn) " +
                "VALUES (@UserId, @Email, @FirstName, @LastName, @EventDateTime)";

            _DbConnection.Execute(query, notification);
        }
    }
}
