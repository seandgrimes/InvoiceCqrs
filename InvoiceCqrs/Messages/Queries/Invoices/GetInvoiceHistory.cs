using System;
using System.Collections.Generic;
using InvoiceCqrs.Domain.ValueObjects;
using MediatR;

namespace InvoiceCqrs.Messages.Queries.Invoices
{
    public class GetInvoiceHistory : IRequest<IList<EventHistoryItem>>
    {
        public Guid InvoiceId { get; set; }
    }
}
