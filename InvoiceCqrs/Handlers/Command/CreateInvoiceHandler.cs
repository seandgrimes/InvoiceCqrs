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
    public class CreateInvoiceHandler : IRequestHandler<CreateInvoice, Invoice>
    {
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public CreateInvoiceHandler(IMediator mediator, Store store)
        {
            _Mediator = mediator;
            _Stream = store.Open(Stores.Invoices);
        }

        public Invoice Handle(CreateInvoice message)
        {
            _Stream.Write(message.Id, new InvoiceCreated
            {
                Id = message.Id,
                InvoiceNumber = message.InvoiceNumber
            });

            return _Mediator.Send(new GetInvoice
            {
                Id = message.Id
            });
        }
    }
}
