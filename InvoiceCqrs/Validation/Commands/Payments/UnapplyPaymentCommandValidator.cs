using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Messages.Commands.Payments;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Messages.Queries.Payments;
using MediatR;

namespace InvoiceCqrs.Validation.Commands.Payments
{
    public class UnapplyPaymentCommandValidator : IValidator<UnapplyPayment>
    {
        private readonly IMediator _Mediator;

        public UnapplyPaymentCommandValidator(IMediator mediator)
        {
            _Mediator = mediator;
        }

        public IList<string> Validate(UnapplyPayment entity)
        {
            var errors = new List<string>();

            var payment = _Mediator.Send(new GetPayment {Id = entity.PaymentId});
            var lineItem = _Mediator.Send(new GetLineItem {Id = entity.LineItemId});

            if (payment == null)
            {
                errors.Add($"Payment {entity.PaymentId} does not exist");
            }

            if (lineItem == null)
            {
                errors.Add($"Line Item {entity.LineItemId} does not exist");
            }

            if (entity.Amount <= 0)
            {
                errors.Add("Amount must be greater than 0");
            }

            return errors;
        }
    }
}
