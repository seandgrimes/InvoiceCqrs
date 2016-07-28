using System;

namespace InvoiceCqrs.Domain.Entities
{
    public class LineItem : Entity
    {
        public Guid InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool IsPaid { get; set; }
    }
}
