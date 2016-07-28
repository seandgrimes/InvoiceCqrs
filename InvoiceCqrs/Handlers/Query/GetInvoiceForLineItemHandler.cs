using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query
{
    public class GetInvoiceForLineItemHandler : IRequestHandler<GetInvoiceForLineItem, Invoice>
    {
        public Invoice Handle(GetInvoiceForLineItem message)
        {
            return ReadModel.Invoices.Values.SingleOrDefault(i => i.LineItems.Any(li => li.Id == message.LineItemId));
        }
    }
}
