using System;
using MediatR;

namespace InvoiceCqrs.Messages.Commands.Payments
{
    public class UnapplyPayment : IRequest<bool>
    {
        public decimal Amount { get; set; }

        public Guid LineItemId { get; set; }

        public Guid PaymentId { get; set; }

        public Guid UnappliedById { get; set; }
    }
}
