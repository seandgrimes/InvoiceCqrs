using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;

namespace InvoiceCqrs.Messages.Events.Invoices
{
    public class LineItemPaid : IEvent<LineItem>, IVisitable<IInvoiceEventVisitor>
    {
        public DateTime EventDateTime { get; } = DateTime.Now;

        public Guid LineItemId { get; set; }

        public Guid PaymentId { get; set; }

        public void Apply(LineItem target)
        {
            target.IsPaid = true;
        }

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}