using System.Linq;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoiceForLineItemHandler : IRequestHandler<GetInvoiceForLineItem, Invoice>
    {
        public Invoice Handle(GetInvoiceForLineItem message)
        {
            return ReadModel.Invoices.Values.SingleOrDefault(i => i.LineItems.Any(li => li.Id == message.LineItemId));
        }
    }
}
