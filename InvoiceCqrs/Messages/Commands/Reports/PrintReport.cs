using System;
using MediatR;

namespace InvoiceCqrs.Messages.Commands.Reports
{
    public class PrintReport : IRequest<bool>
    {
        public Guid PrintedById { get; set; }

        public Guid RecordId { get; set; }

        public Guid ReportId { get; set; }

        public Guid TableId { get; set; }

    }
}
