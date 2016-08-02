using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Commands.Invoices
{
    public class CreateInvoice : IRequest<Invoice>
    {
        public Guid CompanyId { get; set; }

        public Guid CreatedById { get; set; }

        public Guid Id { get; set; }

        public string InvoiceNumber { get; set; }
    }
}
