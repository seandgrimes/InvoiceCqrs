using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceCqrs.Messages.Commands.Invoices;
using InvoiceCqrs.Messages.Queries.Companies;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Messages.Queries.Users;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Validation.Commands.Invoices
{
    public class CreateInvoiceCommandValidator : IValidator<CreateInvoice>
    {
        private readonly IMediator _Mediator;

        public CreateInvoiceCommandValidator(IMediator mediator)
        {
            _Mediator = mediator;
        }

        public IList<string> Validate(CreateInvoice entity)
        {
            var errors = new List<string>();
            var user = _Mediator.Send(new GetUser {UserId = entity.CreatedById});
            var invoice = _Mediator.Send(new GetInvoice {Id = entity.Id});
            var company = _Mediator.Send(new GetCompany {CompanyId = entity.CompanyId});

            if (entity.Id == default(Guid))
            {
                errors.Add("Must specify a value for the CompanyId property");
            }

            if (entity.Id != default(Guid) && invoice != null)
            {
                errors.Add($"An invoice with an id of {entity.Id} already exists");
            }

            if (entity.CompanyId == default(Guid))
            {
                errors.Add("Must specify a value for the CompanyId property");
            }

            if (entity.CompanyId != default(Guid) && company == null)
            {
                errors.Add($"The company {entity.CompanyId} does not exist");
            }

            if (entity.CreatedById == default(Guid))
            {
                errors.Add("Must specify a value for the CreatedById property");
            }

            if (entity.CreatedById != default(Guid) && user == null)
            {
                errors.Add($"The user {entity.CreatedById} does not exist");
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
