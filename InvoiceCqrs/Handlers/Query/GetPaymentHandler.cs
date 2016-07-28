using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain;
using InvoiceCqrs.Messages.Queries;
using MediatR;

namespace InvoiceCqrs.Handlers.Query
{
    public class GetPaymentHandler : IRequestHandler<GetPayment, Payment>
    {
        public Payment Handle(GetPayment message)
        {
            return ReadModel.Payments[message.Id];
        }
    }
}
