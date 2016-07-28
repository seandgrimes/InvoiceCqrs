using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain.ValueObjects;
using MediatR;

namespace InvoiceCqrs.Messages.Queries
{
    public class GetInvoiceHistory : IRequest<IList<EventHistoryItem>>
    {
        public Guid InvoiceId { get; set; }
    }
}
