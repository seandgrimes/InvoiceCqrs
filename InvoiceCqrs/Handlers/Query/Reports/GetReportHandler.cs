using System.Linq;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Reports;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Reports
{
    public class GetReportHandler : IRequestHandler<GetReport, Report>
    {
        private readonly IUnitOfWork _UnitOfWork;
        
        public GetReportHandler(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public Report Handle(GetReport message)
        {
            const string query =
                @"  SELECT r.Id, r.Name
                    FROM Reports.Report r
                    WHERE r.Id = @ReportId";

            return _UnitOfWork.Query<Report>(query, message).SingleOrDefault();
        }
    }
}