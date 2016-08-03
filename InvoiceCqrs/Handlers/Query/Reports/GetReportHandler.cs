using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Reports;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Reports
{
    public class GetReportHandler : IRequestHandler<GetReport, Report>
    {
        private readonly IDbConnection _DbConnection;

        public GetReportHandler(IDbConnection dbConnection)
        {
            _DbConnection = dbConnection;
        }

        public Report Handle(GetReport message)
        {
            const string query =
                @"  SELECT r.Id, r.Name
                    FROM Reports.Report r
                    WHERE r.Id = @ReportId";

            return _DbConnection.Query<Report>(query, message).SingleOrDefault();
        }
    }
}