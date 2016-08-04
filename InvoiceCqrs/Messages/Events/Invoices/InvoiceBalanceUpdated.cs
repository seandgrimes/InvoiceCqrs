using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;

namespace InvoiceCqrs.Messages.Events.Invoices
{
    public class InvoiceBalanceUpdated : IEvent<Invoice>, IVisitable<IInvoiceEventVisitor, EventHistoryItem>
    {
        public decimal Amount { get; set; }

        public DateTime EventDate { get; } = DateTime.Now;

        public Guid InvoiceId { get; set; }

        public Guid LineItemId { get; set; }

        public Guid PaymentId { get; set; }

        public Guid UpdatedById { get; set; }

        public EventHistoryItem Accept(IInvoiceEventVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public void Apply(Invoice target)
        {
            target.Balance += Amount;
        }
    }
}
