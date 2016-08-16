using InvoiceCqrs.Messages.Events.Companies;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Event
{
    public class UpdateCompanyReadModelEventHandlers : INotificationHandler<CompanyCreated>
    {
        private readonly IUnitOfWork _UnitOfWork;
        
        public UpdateCompanyReadModelEventHandlers(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public void Handle(CompanyCreated notification)
        {
            const string query = @"
                INSERT INTO Companies.Company (Id, Name, Addr1, Addr2, City, State, ZipCode)
                VALUES (@CompanyId, @Name, @Addr1, @Addr2, @City, @State, @ZipCode)";

            _UnitOfWork.Execute(query, notification);
        }
    }
}
