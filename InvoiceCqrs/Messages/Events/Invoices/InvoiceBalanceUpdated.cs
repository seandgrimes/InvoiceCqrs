using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;

namespace InvoiceCqrs.Messages.Events.Invoices
{
    public class InvoiceBalanceUpdated : IEvent<Invoice>, IVisitable<IInvoiceEventVisitor>
    {
        public decimal Amount { get; set; }

        public DateTime EventDateTime { get; } = DateTime.Now;

        public Guid InvoiceId { get; set; }

        public Guid LineItemId { get; set; }

        public Guid PaymentId { get; set; }

        public Guid UpdatedById { get; set; }

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Apply(Invoice target)
        {
            target.Balance += Amount;
        }
    }
}
