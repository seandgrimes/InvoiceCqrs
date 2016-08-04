using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;

namespace InvoiceCqrs.Messages.Events.Invoices
{
    public class InvoicePrinted : IEvent, IVisitable<IInvoiceEventVisitor>
    {
        public DateTime EventDate { get; } = DateTime.UtcNow;

        public Guid InvoiceId { get; set; }

        public bool IsReprint { get; set; }

        public Guid ReportId { get; set; }

        public Guid PrintedById { get; set; }

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
