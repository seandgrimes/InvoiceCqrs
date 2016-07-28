using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain;
using MediatR;

namespace InvoiceCqrs.Messages.Queries
{
    public class GetInvoiceForLineItem : IRequest<Invoice>
    {
        public Guid LineItemId { get; set; }
    }
}
