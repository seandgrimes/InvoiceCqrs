using System;
using InvoiceCqrs.Domain;
using InvoiceCqrs.EventStore;
using InvoiceCqrs.Messages.Commands;
using InvoiceCqrs.Messages.Events;
using MediatR;

namespace InvoiceCqrs.Handlers.Command
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