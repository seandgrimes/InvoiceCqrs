using System;
using System.Collections.Generic;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Shared;
using MediatR;

namespace InvoiceCqrs.Messages.Queries.Invoices
{
    public class GetInvoices : IRequest<IList<Invoice>>
    {
        public Guid CompanyId { get; set; }

        public string InvoiceNumber { get; set; }

        public SearchOptions SearchOption { get; set; }
    }
}
