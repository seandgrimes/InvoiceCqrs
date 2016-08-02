using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;

namespace InvoiceCqrs.Messages.Events.Payments
{
    public class PaymentUnapplied : IEvent, IVisitable<IInvoiceEventVisitor>
    {
        public Guid PaymentId { get; set; }

        public Guid LineItemId { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventDateTime { get; } = DateTime.Now;

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}