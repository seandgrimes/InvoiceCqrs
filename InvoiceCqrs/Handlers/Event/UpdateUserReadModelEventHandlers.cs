using InvoiceCqrs.Messages.Events.Users;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Event
{
    public class UpdateUserReadModelEventHandlers : INotificationHandler<UserCreated>
    {
        private readonly IUnitOfWork _UnitOfWork;
        
        public UpdateUserReadModelEventHandlers(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public void Handle(UserCreated notification)
        {
            const string query =
                "INSERT INTO Users.[User] (Id, Email, FirstName, LastName, CreatedOn) " +
                "VALUES (@UserId, @Email, @FirstName, @LastName, @EventDate)";

            _UnitOfWork.Execute(query, notification);
        }
    }
}
