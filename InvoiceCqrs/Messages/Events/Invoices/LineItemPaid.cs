using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;

namespace InvoiceCqrs.Messages.Events.Invoices
{
    public class LineItemPaid : IEvent<LineItem>, IVisitable<IInvoiceEventVisitor, EventHistoryItem>
    {
        public DateTime EventDate { get; } = DateTime.UtcNow;

        public Guid LineItemId { get; set; }

        public Guid PaymentId { get; set; }

        public void Apply(LineItem target)
        {
            target.IsPaid = true;
        }

        public EventHistoryItem Accept(IInvoiceEventVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}