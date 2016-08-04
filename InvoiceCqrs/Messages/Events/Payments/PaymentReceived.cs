using System;
using InvoiceCqrs.Domain.Entities;

namespace InvoiceCqrs.Messages.Events.Payments
{
    public class PaymentReceived : IEvent<Payment>
    {
        public Guid Id { get; set; }

        public DateTime ReceivedOn { get; set; }

        public Guid ReceivedById { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventDate { get; } = DateTime.UtcNow;

        public void Apply(Payment target)
        {
            target.Id = Id;
            target.ReceivedOn = ReceivedOn;
            target.ReceivedBy = new User {Id = ReceivedById};
            target.Balance = Amount;
            target.Amount = Amount;
        }
    }
}