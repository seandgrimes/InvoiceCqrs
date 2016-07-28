using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace InvoiceCqrs.Messages.Commands
{
    public class UnapplyPayment : IRequest<bool>
    {
        public decimal Amount { get; set; }

        public Guid LineItemId { get; set; }

        public Guid PaymentId { get; set; }
    }
}
