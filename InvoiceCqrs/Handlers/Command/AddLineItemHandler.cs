using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain;
using InvoiceCqrs.EventStore;
using InvoiceCqrs.Messages.Commands;
using InvoiceCqrs.Messages.Events;
using InvoiceCqrs.Messages.Queries;
using MediatR;

namespace InvoiceCqrs.Handlers.Command
{
    public class AddLineItemHandler : IRequestHandler<AddLineItem, LineItem>
    {
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public AddLineItemHandler(IMediator mediator, Store store)
        {
            _Mediator = mediator;
            _Stream = store.Open(Stores.Invoices);
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

            return _Mediator.Send(new GetLineItem
            {
                Id = message.Id
            });
        }
    }
}
