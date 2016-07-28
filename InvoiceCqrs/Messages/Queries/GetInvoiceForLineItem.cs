using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Queries
{
    public class GetInvoiceForLineItem : IRequest<Invoice>
    {
        public Guid LineItemId { get; set; }
    }
}