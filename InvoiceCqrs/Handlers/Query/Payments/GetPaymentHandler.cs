using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Payments;
using InvoiceCqrs.Messages.Queries.Users;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Payments
{
    public class GetPaymentHandler : IRequestHandler<GetPayment, Payment>
    {
        private readonly IDbConnection _DbConnection;
        private readonly IMediator _Mediator;

        public GetPaymentHandler(IDbConnection dbConnection, IMediator mediator)
        {
            _DbConnection = dbConnection;
            _Mediator = mediator;
        }

        public Payment Handle(GetPayment message)
        {
            const string query =
                "SELECT p.Id, p.ReceivedOn, p.ReceivedById, p.Amount, p.Balance " +
                "FROM Accounting.Payment p " +
                "WHERE p.Id = @Id ";

            var payment = _DbConnection.Query<Payment>(query, message).SingleOrDefault();
            if (payment == null)
            {
                return null;
            }

            payment.ReceivedBy = _Mediator.Send(new GetUser {UserId = payment.ReceivedById});
            return payment;
        }
    }
}
