using System;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;

namespace InvoiceCqrs.Messages.Events.Payments
{
    public class PaymentApplied : IEvent, IVisitable<IInvoiceEventVisitor, EventHistoryItem>
    {
        public Guid PaymentId { get; set; }

        public Guid LineItemId { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventDate { get; } = DateTime.UtcNow;

        public EventHistoryItem Accept(IInvoiceEventVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}