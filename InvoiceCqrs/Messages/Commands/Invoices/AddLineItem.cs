using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Commands.Invoices
{
    public class AddLineItem : IRequest<LineItem>
    {
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public Guid Id { get; set; }

        public Guid InvoiceId { get; set; }
    }
}
