using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain;
using MediatR;

namespace InvoiceCqrs.Messages.Commands
{
    public class AddLineItem : IRequest<LineItem>
    {
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public Guid Id { get; set; }

        public Guid InvoiceId { get; set; }
    }
}
