using System;
using InvoiceCqrs.Domain;
using InvoiceCqrs.Visitors;

namespace InvoiceCqrs.Messages.Events
{
    public class PaymentUnapplied : IEvent<Invoice>, IVisitable<IInvoiceEventVisitor>
    {
        public int PaymentId { get; set; }

        public int LineItemId { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventDateTime { get; } = DateTime.Now;

        public void Apply(Invoice target)
        {
            target.Balance += Amount;
        }

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}