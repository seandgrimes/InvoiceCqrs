using System;
using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Invoices;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoiceForLineItemHandler : IRequestHandler<GetInvoiceForLineItem, Invoice>
    {
        private readonly IDbConnection _DbConnection;
        private readonly IMediator _Mediator;

        public GetInvoiceForLineItemHandler(IDbConnection dbConnection, IMediator mediator)
        {
            _DbConnection = dbConnection;
            _Mediator = mediator;
        }

        public Invoice Handle(GetInvoiceForLineItem message)
        {
            const string query = @"
                SELECT li.InvoiceId
                FROM Accounting.LineItem li
                WHERE li.Id = @LineItemId;";

            var invoiceId = _DbConnection.Query<Guid?>(query, message).SingleOrDefault();
            if (!invoiceId.HasValue)
            {
                return null;
            }

            return _Mediator.Send(new GetInvoice {Id = invoiceId.Value});
        }
    }
}
