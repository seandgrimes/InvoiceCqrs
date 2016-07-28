using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Commands
{
    public class CreateInvoice : IRequest<Invoice>
    {
        public Guid Id { get; set; }

        public string InvoiceNumber { get; set; }
    }
}
