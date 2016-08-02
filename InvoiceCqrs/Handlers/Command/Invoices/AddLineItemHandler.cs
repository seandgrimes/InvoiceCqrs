using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands;
using InvoiceCqrs.Messages.Commands.Invoices;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Invoices
{
    public class AddLineItemHandler : IRequestHandler<AddLineItem, LineItem>
    {
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public AddLineItemHandler(IMediator mediator, Store store)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);
        }

        public LineItem Handle(AddLineItem message)
        {
            _Stream.Write(message.InvoiceId, new LineItemAdded
            {
                Id = message.Id,
                Amount = message.Amount,
                Description = message.Description,
                InvoiceId = message.InvoiceId
            });

            _Stream.Write(message.InvoiceId, new InvoiceBalanceUpdated
            {
                Amount = message.Amount,
                InvoiceId = message.InvoiceId
            });

            return _Mediator.Send(new GetLineItem
            {
                Id = message.Id
            });
        }
    }
}
