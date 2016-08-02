using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Queries.Invoices
{
    public class GetInvoice : IRequest<Invoice>
    {
        public Guid Id { get; set; }
    }
}
