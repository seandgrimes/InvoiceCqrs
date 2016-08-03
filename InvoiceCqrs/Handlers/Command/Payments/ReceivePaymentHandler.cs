using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands.Payments;
using InvoiceCqrs.Messages.Events.Payments;
using InvoiceCqrs.Messages.Queries.Payments;
using InvoiceCqrs.Persistence;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Payments
{
    public class ReceivePaymentHandler : IRequestHandler<ReceivePayment, Payment>
    {
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public ReceivePaymentHandler(Store store, IMediator mediator)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);
        }

        public Payment Handle(ReceivePayment message)
        {
            _Stream.Write(message.Id, new PaymentReceived
            {
                Amount = message.Amount,
                Id = message.Id,
                ReceivedOn = message.ReceivedOn,
                ReceivedById = message.ReceivedById
            });

            return _Mediator.Send(new GetPayment {Id = message.Id});
        }
    }
}