using System.Data;
using Dapper;
using InvoiceCqrs.Messages.Events.Companies;
using MediatR;

namespace InvoiceCqrs.Handlers.Event
{
    public class UpdateCompanyReadModelEventHandlers : INotificationHandler<CompanyCreated>
    {
        private readonly IDbConnection _DbConnection;

        public UpdateCompanyReadModelEventHandlers(IDbConnection dbConnection)
        {
            _DbConnection = dbConnection;
        }

        public void Handle(CompanyCreated notification)
        {
            const string query = @"
                INSERT INTO Companies.Company (Id, Name, Addr1, Addr2, City, State, ZipCode)
                VALUES (@CompanyId, @Name, @Addr1, @Addr2, @City, @State, @ZipCode)";

            _DbConnection.Execute(query, notification);
        }
    }
}
