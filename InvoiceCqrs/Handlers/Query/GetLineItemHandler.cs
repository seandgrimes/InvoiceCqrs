using InvoiceCqrs.Domain;
using InvoiceCqrs.Messages.Queries;
using MediatR;

namespace InvoiceCqrs.Handlers.Query
{
    public class GetLineItemHandler : IRequestHandler<GetLineItem, LineItem>
    {
        public LineItem Handle(GetLineItem message)
        {
            return ReadModel.LineItems[message.Id];
        }
    }
}
