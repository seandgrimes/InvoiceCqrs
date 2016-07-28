using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Visitors;

namespace InvoiceCqrs.Messages.Events.Payments
{
    public class PaymentApplied : IEvent<Invoice>, IVisitable<IInvoiceEventVisitor>
    {
        public Guid PaymentId { get; set; }

        public Guid LineItemId { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventDateTime { get; } = DateTime.Now;

        public void Apply(Invoice target)
        {
            // No-op
        }

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}