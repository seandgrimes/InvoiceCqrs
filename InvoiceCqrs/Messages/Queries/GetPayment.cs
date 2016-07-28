using System;
using InvoiceCqrs.Domain;
using MediatR;

namespace InvoiceCqrs.Messages.Queries
{
    public class GetPayment : IRequest<Payment>
    {
        public Guid Id { get; set; }
    }
}
