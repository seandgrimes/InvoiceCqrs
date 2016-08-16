using System;
using System.Linq;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoiceHandler : IRequestHandler<GetInvoice, Invoice>
    {
        private readonly IUnitOfWork _UnitOfWork;
        
        public GetInvoiceHandler(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public Invoice Handle(GetInvoice message)
        {
            // Need to find a much better way to do this... Do we even need fully hydrated objects?
            const string invoiceQuery = @"
                SELECT 
                    i.*,
                    u.*,
                    c.Id, c.Name,
                    c.Id, c.Addr1, c.Addr2, c.City, c.State, c.ZipCode
                FROM Accounting.Invoice i
                JOIN Users.[User] u ON i.CreatedById = u.Id
                JOIN Companies.Company c ON i.CompanyId = c.Id
                WHERE i.Id = @Id;";

            Func<Invoice, User, Company, Address, Invoice> invoiceMapper = (inv, user, company, address) =>
            {
                company.Address = address;
                inv.Company = company;
                inv.CreatedBy = user;

                return inv;
            };

            var invoice = _UnitOfWork.Query(invoiceQuery, invoiceMapper, message).SingleOrDefault();
            if (invoice == null)
            {
                return null;
            }

            const string lineItemQuery = @"
                SELECT 
                    li.*,
                    u.*,
                    i.*,
                    iu.*
                FROM Accounting.LineItem li
                JOIN Users.[User] u ON li.CreatedById = u.Id
                JOIN Accounting.Invoice i ON li.InvoiceId = i.Id
                JOIN Users.[User] iu ON i.CreatedById = iu.Id
                WHERE li.InvoiceId = @Id;";

            Func<LineItem, User, Invoice, User, LineItem> lineItemMapper = (lineItem, lineItemUser, inv, invoiceUser) =>
            {
                lineItem.CreatedBy = lineItemUser;
                lineItem.Invoice = inv;
                lineItem.Invoice.CreatedBy = invoiceUser;

                return lineItem;
            };

            invoice.LineItems = _UnitOfWork.Query(lineItemQuery, lineItemMapper, message)
                .ToList();

            return invoice;
        }
    }
}
