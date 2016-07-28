using System;
using InvoiceCqrs.Domain;
using InvoiceCqrs.Visitors;

namespace InvoiceCqrs.Messages.Events
{
    public class InvoiceCreated : IEvent<Invoice>, IVisitable<IInvoiceEventVisitor>
    {
        public Guid Id { get; set; }

        public string InvoiceNumber { get; set; }

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public DateTime EventDateTime { get; } = DateTime.Now;

        public void Apply(Invoice target)
        {
            target.Id = Id;
            target.InvoiceNumber = InvoiceNumber;
        }
    }
}
