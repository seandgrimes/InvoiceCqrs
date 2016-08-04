using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;

namespace InvoiceCqrs.Messages.Events.Invoices
{
    public class InvoiceCreated : IEvent<Invoice>, IVisitable<IInvoiceEventVisitor, EventHistoryItem>
    {
        public Guid CompanyId { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime EventDate { get; } = DateTime.UtcNow;

        public Guid Id { get; set; }

        public string InvoiceNumber { get; set; }

        public EventHistoryItem Accept(IInvoiceEventVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public void Apply(Invoice target)
        {
            target.Id = Id;
            target.InvoiceNumber = InvoiceNumber;
            target.Company = new Company {Id = CompanyId};
            target.CreatedBy = new User {Id = CreatedById};
        }
    }
}
