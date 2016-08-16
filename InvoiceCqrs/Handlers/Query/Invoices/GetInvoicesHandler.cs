using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Extensions;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoicesHandler : IRequestHandler<GetInvoices, IList<Invoice>>
    {
        private readonly IUnitOfWork _UnitOfWork;

        public GetInvoicesHandler(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IList<Invoice> Handle(GetInvoices message)
        {
            const string queryTemplate = @"
                SELECT 
                    i.*,
                    u.*,
                    c.*
                FROM Accounting.Invoice i
                JOIN Users.[User] u ON i.CreatedById = u.Id
                JOIN Companies.Company c ON i.CompanyId = c.Id
                WHERE
                    (@IsCompanyIdDefault = 1 AND @InvoiceNumber IS NULL) OR
                    ((@IsCompanyIdDefault = 0 AND i.CompanyID = @CompanyID) {CompOper}
                    (@InvoiceNumber IS NOT NULL AND i.InvoiceNumber = @InvoiceNumber))";

            var query = queryTemplate.Replace("{CompOper}", message.SearchOption.ToSqlOperator());
            Func<Invoice, User, Company, Invoice> mapper = (invoice, user, company) =>
            {
                invoice.CreatedBy = user;
                invoice.Company = company;

                return invoice;
            };

            return _UnitOfWork.Query(query, mapper, new
            {
                IsCompanyIdDefault = message.CompanyId == default(Guid) ? 1 : 0,
                message.CompanyId,
                message.InvoiceNumber
            }).ToList();
        }
    }
}
