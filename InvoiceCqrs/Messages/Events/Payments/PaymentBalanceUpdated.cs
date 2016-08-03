using System;
using InvoiceCqrs.Domain.Entities;

namespace InvoiceCqrs.Messages.Events.Payments
{
    public class PaymentBalanceUpdated : IEvent<Payment>
    {
        public decimal Amount { get; set; }
        
        public DateTime EventDateTime { get; } = DateTime.UtcNow;

        public Guid PaymentId { get; set; }

        public Guid UpdatedById { get; set; }

        public void Apply(Payment target)
        {
            target.Balance += Amount;
        }
    }
}
