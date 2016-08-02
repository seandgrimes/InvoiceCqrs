using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetLineItemHandler : IRequestHandler<GetLineItem, LineItem>
    {
        public LineItem Handle(GetLineItem message)
        {
            return ReadModel.LineItems[message.Id];
        }
    }
}
