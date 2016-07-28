using System;

namespace InvoiceCqrs.Domain
{
    public class LineItem : Entity
    {
        public Guid InvoiceId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool IsPaid { get; set; }
    }
}
