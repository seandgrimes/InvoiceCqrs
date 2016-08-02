using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoiceHandler : IRequestHandler<GetInvoice, Invoice>
    {
        private readonly IDbConnection _DbConnection;

        public GetInvoiceHandler(IDbConnection dbConnection)
        {
            _DbConnection = dbConnection;
        }

        public Invoice Handle(GetInvoice message)
        {
            // Need to find a much better way to do this... Do we even need fully hydrated objects?
            const string query = @"
                SELECT i.Id, i.Balance, i.CreatedById, i.InvoiceNumber, i.CompanyId, i.CreatedOn
                FROM Accounting.Invoice i
                WHERE i.Id = @Id;

                SELECT u.Id, u.Email, u.FirstName, u.LastName, u.CreatedOn
                FROM Users.[User] u
                JOIN Accounting.Invoice i ON u.Id = i.CreatedById
                WHERE i.Id = @Id;

                SELECT c.Id, c.Name
                FROM Companies.Company c
                JOIN Accounting.Invoice i ON c.Id = i.CompanyId
                WHERE i.Id = @Id;

                SELECT c.Addr1, c.Addr2, c.City, c.State, c.ZipCode
                FROM Companies.Company c
                JOIN Accounting.Invoice i ON c.Id = i.CompanyId
                WHERE i.Id = @Id;";

            using (var multi = _DbConnection.QueryMultiple(query, message))
            {
                var invoice = multi.Read<Invoice>().SingleOrDefault();
                if (invoice != null)
                {
                    invoice.CreatedBy = multi.Read<User>().SingleOrDefault();
                    invoice.Company = multi.Read<Company>().SingleOrDefault();
                }

                return invoice;
            }
        }
    }
}
