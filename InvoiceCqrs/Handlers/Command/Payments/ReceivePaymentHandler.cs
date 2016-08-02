using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands;
using InvoiceCqrs.Messages.Commands.Payments;
using InvoiceCqrs.Messages.Events.Payments;
using InvoiceCqrs.Persistence;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Payments
{
    public class ReceivePaymentHandler : IRequestHandler<ReceivePayment, Payment>
    {
        private readonly Stream _Stream;

        public ReceivePaymentHandler(Store store)
        {
            _Stream = store.Open(Streams.Invoices);
        }

        public Payment Handle(ReceivePayment message)
        {
            _Stream.Write(message.Id, new PaymentReceived
            {
                Amount = message.Amount,
                Id = message.Id,
                ReceivedOn = message.ReceivedOn
            });

            return ReadModel.Payments[message.Id];
        }
    }
}