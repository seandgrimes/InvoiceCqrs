using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Queries.Reports
{
    public class GetReport : IRequest<Report>
    {
        public Guid ReportId { get; set; }
    }
}
