using System.Data;
using Dapper;
using InvoiceCqrs.Messages.Events.Users;
using MediatR;

namespace InvoiceCqrs.Handlers.Event
{
    public class UpdateUserReadModelEventHandlers : INotificationHandler<UserCreated>
    {
        private readonly IDbConnection _DbConnection;

        public UpdateUserReadModelEventHandlers(IDbConnection dbConnection)
        {
            _DbConnection = dbConnection;
        }

        public void Handle(UserCreated notification)
        {
            const string query =
                "INSERT INTO Users.[User] (Id, Email, FirstName, LastName, CreatedOn) " +
                "VALUES (@UserId, @Email, @FirstName, @LastName, @EventDate)";

            _DbConnection.Execute(query, notification);
        }
    }
}
