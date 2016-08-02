using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query
{
    public class GetReportHandler : IRequestHandler<GetReport, Report>
    {
        public Report Handle(GetReport message)
        {
            return ReadModel.Reports[message.ReportId];
        }
    }
}