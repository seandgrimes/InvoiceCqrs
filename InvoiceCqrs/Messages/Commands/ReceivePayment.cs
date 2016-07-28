using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain;
using MediatR;

namespace InvoiceCqrs.Messages.Commands
{
    public class ReceivePayment : IRequest<Payment>
    {
        public Guid Id { get; set; }

        public DateTime ReceivedOn { get; set; }

        public decimal Amount { get; set; }
    }
}
