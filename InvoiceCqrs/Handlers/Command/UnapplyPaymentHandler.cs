﻿using InvoiceCqrs.Messages.Commands;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Events.Payments;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command
{
    public class UnapplyPaymentHandler : IRequestHandler<UnapplyPayment, bool>
    {
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public UnapplyPaymentHandler(IMediator mediator, Store store)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);
        }

        public bool Handle(UnapplyPayment message)
        {
            var invoice = _Mediator.Send(new GetInvoiceForLineItem {LineItemId = message.LineItemId});

            _Stream.Write(invoice.Id, new PaymentUnapplied
            {
                Amount = message.Amount,
                LineItemId = message.LineItemId,
                PaymentId = message.PaymentId
            });

            _Stream.Write(invoice.Id, new InvoiceBalanceUpdated
            {
                Amount = message.Amount,
                InvoiceId = invoice.Id,
                PaymentId = message.PaymentId
            });

            return true;
        }
    }
}