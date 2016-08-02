using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace InvoiceCqrs.Messages.Commands
{
    public class PrintReport : IRequest<bool>
    {
        public Guid PrintedById { get; set; }

        public Guid RecordId { get; set; }

        public Guid ReportId { get; set; }

        public Guid TableId { get; set; }

    }
}
