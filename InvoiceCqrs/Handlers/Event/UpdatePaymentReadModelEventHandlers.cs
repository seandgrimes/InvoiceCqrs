using System;
using System.Data;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Events.Payments;
using InvoiceCqrs.Messages.Queries.Payments;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Event
{
    public class UpdatePaymentReadModelEventHandlers : INotificationHandler<PaymentReceived>, INotificationHandler<PaymentBalanceUpdated>
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMediator _Mediator;

        public UpdatePaymentReadModelEventHandlers(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _UnitOfWork = unitOfWork;
            _Mediator = mediator;
        }

        public void Handle(PaymentReceived notification)
        {
            var payment = new Payment();
            notification.Apply(payment);

            const string query =
                "INSERT INTO Accounting.Payment (Id, ReceivedOn, ReceivedById, Amount, Balance)" +
                "VALUES (@Id, @ReceivedOn, @ReceivedById, @Amount, @Balance)";

            _UnitOfWork.Execute(query, new
            {
                payment.Id,
                payment.ReceivedOn,
                ReceivedById = payment.ReceivedBy.Id,
                payment.Amount,
                payment.Balance
            });
        }

        public void Handle(PaymentBalanceUpdated notification)
        {
            var payment = _Mediator.Send(new GetPayment {Id = notification.PaymentId});
            if (payment == null)
            {
                throw new Exception($"Payment {notification.PaymentId} does not exist");
            }
            notification.Apply(payment);

            const string query =
                @"  UPDATE Accounting.Payment
                    SET Balance = @Balance
                    WHERE Id = @Id";

            _UnitOfWork.Execute(query, payment);
        }
    }
}
