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
    public class GetPaymentHandler : IRequestHandler<GetPayment, Payment>
    {
        public Payment Handle(GetPayment message)
        {
            return ReadModel.Payments[message.Id];
        }
    }
}
