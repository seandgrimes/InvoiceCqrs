using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Queries
{
    public class GetReport : IRequest<Report>
    {
        public Guid ReportId { get; set; }
    }
}
