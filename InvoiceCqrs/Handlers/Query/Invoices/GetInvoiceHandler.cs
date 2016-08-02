using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoiceHandler : IRequestHandler<GetInvoice, Invoice>
    {
        public Invoice Handle(GetInvoice message)
        {
            return ReadModel.Invoices[message.Id];
        }
    }
}
