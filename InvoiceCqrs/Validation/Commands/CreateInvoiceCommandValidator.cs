using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceCqrs.Messages.Commands;

namespace InvoiceCqrs.Validation.Commands
{
    public class CreateInvoiceCommandValidator : IValidator<CreateInvoice>
    {
        public IList<string> Validate(CreateInvoice entity)
        {
            var errors = new List<string>();

            if (entity.Id == default(Guid))
            {
                errors.Add("Must specify a value for the Id property");
            }

            if (entity.Id != default(Guid) && ReadModel.Invoices.ContainsKey(entity.Id))
            {
                errors.Add($"An invoice with an id of {entity.Id} already exists");
            }

            if (string.IsNullOrEmpty(entity.InvoiceNumber))
            {
                errors.Add("Must specify a value for the InvoiceNumber property");
            }

            if (!string.IsNullOrEmpty(entity.InvoiceNumber) &&
                ReadModel.Invoices.Values.Any(i => i.InvoiceNumber == entity.InvoiceNumber))
            {
                errors.Add($"An invoice number of {entity.InvoiceNumber} already exists");
            }

            return errors;
        }
    }
}
