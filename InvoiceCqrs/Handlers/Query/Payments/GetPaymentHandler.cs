using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Messages.Queries.Payments;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Payments
{
    public class GetPaymentHandler : IRequestHandler<GetPayment, Payment>
    {
        public Payment Handle(GetPayment message)
        {
            return ReadModel.Payments[message.Id];
        }
    }
}
