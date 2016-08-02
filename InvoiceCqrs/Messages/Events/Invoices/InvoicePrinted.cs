using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Visitors;

namespace InvoiceCqrs.Messages.Events.Invoices
{
    public class InvoicePrinted : IEvent, IVisitable<IInvoiceEventVisitor>
    {
        public Guid InvoiceId { get; set; }

        public bool IsReprint { get; set; }

        public Guid ReportId { get; set; }

        public Guid PrintedById { get; set; }

        public DateTime EventDateTime { get; } = DateTime.Now;

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
