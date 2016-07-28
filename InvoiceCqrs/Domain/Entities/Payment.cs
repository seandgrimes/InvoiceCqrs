using System;

namespace InvoiceCqrs.Domain.Entities
{
    public class Payment : Entity
    {
        public DateTime ReceivedOn { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
    }
}
