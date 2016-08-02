using System;
using MediatR;

namespace InvoiceCqrs.Messages.Commands.Payments
{
    public class ApplyPayment : IRequest<bool>
    {
        public decimal Amount { get; set; }

        public Guid Id { get; set; }

        public Guid LineItemId { get; set; }

        public Guid PaymentId { get; set; }
    }
}
