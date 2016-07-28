using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query
{
    public class GetInvoiceHandler : IRequestHandler<GetInvoice, Invoice>
    {
        public Invoice Handle(GetInvoice message)
        {
            return ReadModel.Invoices[message.Id];
        }
    }
}
