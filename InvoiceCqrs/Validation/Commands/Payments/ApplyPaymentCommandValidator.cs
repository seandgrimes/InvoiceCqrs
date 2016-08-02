using System;
using System.Collections.Generic;
using InvoiceCqrs.Messages.Commands.Payments;

namespace InvoiceCqrs.Validation.Commands.Payments
{
    public class ApplyPaymentCommandValidator : IValidator<ApplyPayment>
    {
        public IList<string> Validate(ApplyPayment entity)
        {
            var errors = new List<string>();

            if (entity.LineItemId == default(Guid))
            {
                errors.Add("The LineItemId property must have a value");
            }

            if (entity.PaymentId == default(Guid))
            {
                errors.Add("The PaymentId property must have a value");
            }

            if (entity.Amount <= 0)
            {
                errors.Add("The payment amount must have a value greater than 0");
            }

            return errors;
        }
    }
}
