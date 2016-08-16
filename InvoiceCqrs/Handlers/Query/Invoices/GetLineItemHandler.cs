using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetLineItemHandler : IRequestHandler<GetLineItem, LineItem>
    {
        private readonly IUnitOfWork _UnitOfWork;

        public GetLineItemHandler(IDbConnection dbConnection, IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public LineItem Handle(GetLineItem message)
        {
            const string query = @"
                -- Invoice
                SELECT i.Id, i.Balance, i.InvoiceNumber, i.CreatedOn
                FROM Accounting.Invoice i
                JOIN Accounting.LineItem li ON i.Id = li.InvoiceId
                WHERE li.Id = @Id;

                -- Invoice Company
                SELECT c.Id, c.Name
                FROM Companies.Company c
                JOIN Accounting.Invoice i ON c.Id = i.CompanyId
                JOIN Accounting.LineItem li ON i.Id = li.InvoiceId
                WHERE li.Id = @Id;

                -- Invoice Company Address
                SELECT c.Addr1, c.Addr2, c.City, c.State, c.ZipCode
                FROM Companies.Company c
                JOIN Accounting.Invoice i ON c.Id = i.CompanyId
                JOIN Accounting.LineItem li ON i.Id = li.InvoiceId
                WHERE li.Id = @Id;

                -- Invoice Created By
                SELECT u.Id, u.Email, u.FirstName, u.LastName
                FROM Users.[User] u
                JOIN Accounting.Invoice i ON u.Id = i.CreatedById
                JOIN Accounting.LineItem li ON i.Id = li.InvoiceId
                WHERE li.Id = @Id;

                -- Line Item
                SELECT li.Id, li.Description, li.Amount, li.IsPaid, li.CreatedOn
                FROM Accounting.LineItem li
                WHERE li.Id = @Id;

                -- Line Item Created By
                SELECT u.Id, u.Email, u.FirstName, u.LastName
                FROM Users.[User] u
                JOIN Accounting.LineItem li ON u.Id = li.CreatedById
                WHERE li.Id = @Id;";

            using (var multi = _UnitOfWork.QueryMultiple(query, message))
            {
                var invoice = multi.Read<Invoice>().SingleOrDefault();
                var company = multi.Read<Company>().SingleOrDefault();
                var companyAddress = multi.Read<Address>().SingleOrDefault();
                var invoiceCreatedBy = multi.Read<User>().SingleOrDefault();
                var lineItem = multi.Read<LineItem>().SingleOrDefault();
                var lineItemCreatedBy = multi.Read<User>().SingleOrDefault();

                if (lineItem == null)
                {
                    return null;
                }

                lineItem.CreatedBy = lineItemCreatedBy;
                lineItem.Invoice = invoice;

                if (company != null) company.Address = companyAddress;
                if (invoice != null)
                {
                    invoice.Company = company;
                    invoice.CreatedBy = invoiceCreatedBy;
                    invoice.LineItems.Add(lineItem);
                }

                return lineItem;
            }
        }
    }
}
