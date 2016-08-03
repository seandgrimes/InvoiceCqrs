using InvoiceCqrs.Messages.Commands.Payments;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Events.Payments;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Payments
{
    public class ApplyPaymentHandler : IRequestHandler<ApplyPayment, bool>
    {
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;
        
        public ApplyPaymentHandler(Store store, IMediator mediator)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);
        }

        public bool Handle(ApplyPayment message)
        {
            var invoice = _Mediator.Send(new GetInvoiceForLineItem { LineItemId = message.LineItemId });

            _Stream.Write(invoice.Id, new PaymentApplied
            {
                Amount = message.Amount,
                LineItemId = message.LineItemId,
                PaymentId = message.PaymentId
            });

            _Stream.Write(invoice.Id, new LineItemPaid
            {
                LineItemId = message.LineItemId,
                PaymentId = message.PaymentId
            });

            _Stream.Write(invoice.Id, new InvoiceBalanceUpdated
            {
                Amount = -1 * message.Amount,
                InvoiceId = invoice.Id,
                LineItemId = message.LineItemId,
                PaymentId = message.PaymentId,
                UpdatedById = message.AppliedById
            });

            _Stream.Write(message.PaymentId, new PaymentBalanceUpdated
            {
                Amount = -1 * message.Amount,
                PaymentId = message.PaymentId
            });

            return true;
        }
    }
}
