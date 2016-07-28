using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Visitors;

namespace InvoiceCqrs.Messages.Events.Invoices
{
    public class LineItemAdded : IEvent<LineItem>, IVisitable<IInvoiceEventVisitor>
    {
        public Guid Id { get; set; }

        public Guid InvoiceId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventDateTime { get; } = DateTime.Now;

        public void Apply(LineItem target)
        {
            target.Amount = Amount;
            target.Id = Id;
            target.Description = Description;
            target.InvoiceId = InvoiceId;
        }

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
