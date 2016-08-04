using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;

namespace InvoiceCqrs.Messages.Events.Invoices
{
    public class LineItemAdded : IEvent<LineItem>, IVisitable<IInvoiceEventVisitor>
    {
        public Guid Id { get; set; }

        public Guid InvoiceId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventDate { get; } = DateTime.UtcNow;

        public Guid CreatedById { get; set; }

        public void Apply(LineItem target)
        {
            target.Amount = Amount;
            target.Id = Id;
            target.Description = Description;
            target.InvoiceId = InvoiceId;
            target.CreatedBy = new User {Id = CreatedById};
        }

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
