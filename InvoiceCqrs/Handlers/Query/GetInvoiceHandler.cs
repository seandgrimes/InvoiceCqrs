using InvoiceCqrs.Domain;
using InvoiceCqrs.Messages.Queries;
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
