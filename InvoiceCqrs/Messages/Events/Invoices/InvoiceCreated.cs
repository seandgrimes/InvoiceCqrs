using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;

namespace InvoiceCqrs.Messages.Events.Invoices
{
    public class InvoiceCreated : IEvent<Invoice>, IVisitable<IInvoiceEventVisitor>
    {
        public Guid CompanyId { get; set; }

        public Guid CreatedById { get; set; }

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
            target.Company = new Company {Id = CompanyId};
            target.CreatedBy = new User {Id = CreatedById};
        }
    }
}
