using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Extensions;
using InvoiceCqrs.Messages.Queries.Invoices;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoicesHandler : IRequestHandler<GetInvoices, IList<Invoice>>
    {
        private readonly IDbConnection _DbConnection;

        public GetInvoicesHandler(IDbConnection dbConnection)
        {
            _DbConnection = dbConnection;
        }

        public IList<Invoice> Handle(GetInvoices message)
        {
            const string query = @"
                SELECT i.Id, i.Balance, i.CreatedById, i.InvoiceNumber, i.CompanyId, i.CreatedOn
                FROM Accounting.Invoice i
                WHERE
                    (@IsCompanyIdDefault = 1 OR i.CompanyID = @CompanyID) {CompOper}
                    (@InvoiceNumber IS NULL OR i.InvoiceNumber = @InvoiceNumber)";

            return _DbConnection.Query<Invoice>(query.Replace("{CompOper}", message.SearchOption.ToSqlOperator()), new
            {
                IsCompanyIdDefault = message.CompanyId == default(Guid) ? 1 : 0,
                message.CompanyId,
                message.InvoiceNumber
            }).ToList();
        }
    }
}
