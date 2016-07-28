using System;
using InvoiceCqrs.Domain;
using InvoiceCqrs.Visitors;

namespace InvoiceCqrs.Messages.Events
{
    public class PaymentReceived : IEvent<Payment>, IVisitable<IInvoiceEventVisitor>
    {
        public Guid Id { get; set; }

        public DateTime ReceivedOn { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventDateTime { get; } = DateTime.Now;

        public void Apply(Payment target)
        {
            target.ReceivedOn = ReceivedOn;
            target.Balance = Amount;
            target.Amount = Amount;
        }

        public void Accept(IInvoiceEventVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}