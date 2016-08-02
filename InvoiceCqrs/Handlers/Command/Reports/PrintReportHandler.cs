using InvoiceCqrs.Messages.Commands;
using InvoiceCqrs.Messages.Commands.Reports;
using InvoiceCqrs.Messages.Events.Reports;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Reports
{
    public class PrintReportHandler : IRequestHandler<PrintReport, bool>
    {
        private readonly Stream _Stream;

        public PrintReportHandler(Store store)
        {
            _Stream = store.Open(Streams.Invoices);
        }

        public bool Handle(PrintReport message)
        {
            // Code for queuing up report for printing...

            _Stream.Write(message.ReportId, new ReportPrinted
            {
                IsReprint = false,
                PrintedById = message.PrintedById,
                RecordId = message.RecordId,
                ReportId = message.ReportId,
                TableId = message.TableId
            });

            return true;
        }
    }
}
