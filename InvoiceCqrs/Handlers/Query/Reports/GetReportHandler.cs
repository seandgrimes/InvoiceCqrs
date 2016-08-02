using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Messages.Queries.Reports;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Reports
{
    public class GetReportHandler : IRequestHandler<GetReport, Report>
    {
        public Report Handle(GetReport message)
        {
            return ReadModel.Reports[message.ReportId];
        }
    }
}