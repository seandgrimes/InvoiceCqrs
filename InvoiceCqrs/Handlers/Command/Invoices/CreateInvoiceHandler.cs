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
    public class CreateInvoiceHandler : IRequestHandler<CreateInvoice, Invoice>
    {
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public CreateInvoiceHandler(IMediator mediator, Store store)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);
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
