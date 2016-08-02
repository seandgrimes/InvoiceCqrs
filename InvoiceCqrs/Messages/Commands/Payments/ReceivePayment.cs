using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Commands.Payments
{
    public class ReceivePayment : IRequest<Payment>
    {
        public Guid Id { get; set; }

        public DateTime ReceivedOn { get; set; }

        public decimal Amount { get; set; }
    }
}
