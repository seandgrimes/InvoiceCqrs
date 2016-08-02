using System;
using System.Data;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Queries.Invoices
{
    public class GetInvoiceForLineItem : IRequest<Invoice>
    {
        public Guid LineItemId { get; set; }
    }
}