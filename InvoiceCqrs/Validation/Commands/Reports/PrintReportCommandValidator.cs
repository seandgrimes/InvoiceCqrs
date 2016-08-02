using System.Collections.Generic;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands.Reports;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Messages.Queries.Reports;
using InvoiceCqrs.Messages.Queries.Users;
using MediatR;

namespace InvoiceCqrs.Validation.Commands.Reports
{
    public class PrintReportCommandValidator : IValidator<PrintReport>
    {
        private readonly IMediator _Mediator;

        public PrintReportCommandValidator(IMediator mediator)
        {
            _Mediator = mediator;
        }

        public IList<string> Validate(PrintReport entity)
        {
            var errors = new List<string>();
            var user = _Mediator.Send(new GetUser {UserId = entity.PrintedById});
            var report = _Mediator.Send(new GetReport {ReportId = entity.ReportId});

            if (user == null)
            {
                errors.Add($"User {entity.PrintedById} does not exist");
            }

            if (report == null)
            {
                errors.Add($"Report {entity.ReportId} does not exist");
            }

            if (entity.TableId == Table.InvoiceTableId)
            {
                ValidateInvoice(entity, errors);
            }

            return errors;
        }

        private void ValidateInvoice(PrintReport entity, ICollection<string> errors)
        {
            var invoice = _Mediator.Send(new GetInvoice {Id = entity.RecordId});
            if (invoice == null)
            {
                errors.Add($"Invoice {entity.RecordId} does not exist");
            }
        }
    }
}
