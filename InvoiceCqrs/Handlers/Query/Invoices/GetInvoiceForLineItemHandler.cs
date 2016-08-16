using System;
using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoiceForLineItemHandler : IRequestHandler<GetInvoiceForLineItem, Invoice>
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMediator _Mediator;

        public GetInvoiceForLineItemHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _UnitOfWork = unitOfWork;
            _Mediator = mediator;
        }

        public Invoice Handle(GetInvoiceForLineItem message)
        {
            const string query = @"
                SELECT li.InvoiceId
                FROM Accounting.LineItem li
                WHERE li.Id = @LineItemId;";

            var invoiceId = _UnitOfWork.Query<Guid?>(query, message).SingleOrDefault();
            if (!invoiceId.HasValue)
            {
                return null;
            }

            return _Mediator.Send(new GetInvoice {Id = invoiceId.Value});
        }
    }
}
