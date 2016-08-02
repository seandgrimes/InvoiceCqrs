using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Queries.Invoices
{
    public class GetLineItem : IRequest<LineItem>
    {
        public Guid Id { get; set; }
    }
}
