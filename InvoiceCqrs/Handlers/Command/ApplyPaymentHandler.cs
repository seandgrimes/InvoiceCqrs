using AutoMapper;
using InvoiceCqrs.EventStore;
using InvoiceCqrs.Handlers.Query;
using InvoiceCqrs.Messages.Commands;
using InvoiceCqrs.Messages.Events;
using InvoiceCqrs.Messages.Queries;
using MediatR;

namespace InvoiceCqrs.Handlers.Command
{
    public class ApplyPaymentHandler : IRequestHandler<ApplyPayment, bool>
    {
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public ApplyPaymentHandler(IMediator mediator, Store store)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);

           Mapper.Initialize(cfg => cfg.CreateMap<ApplyPayment, PaymentApplied>());
        }

        public bool Handle(ApplyPayment message)
        {
            var invoice = _Mediator.Send(new GetInvoiceForLineItem
            {
                LineItemId = message.LineItemId
            });

            _Stream.Write(invoice.Id, Mapper.Map<PaymentApplied>(message));
            return true;
        }
    }
}
