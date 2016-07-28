using System;
using System.Linq;
using InvoiceCqrs.Domain;
using InvoiceCqrs.Visitors;

namespace InvoiceCqrs.Messages.Events
{
    public class PaymentApplied : IEvent<Invoice>, IVisitable<IInvoiceEventVisitor>
    {
        public Guid PaymentId { get; set; }

        public Guid LineItemId { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventDateTime { get; } = DateTime.Now;

        public void Apply(Invoice target)
        {
            target.Balance -= Amount;
        }

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}