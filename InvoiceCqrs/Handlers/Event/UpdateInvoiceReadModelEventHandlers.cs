using AutoMapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Events.Reports;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using InvoiceCqrs.Persistence.EventStore;
using InvoiceCqrs.Util;
using MediatR;

namespace InvoiceCqrs.Handlers.Event
{
    public class UpdateInvoiceReadModelEventHandlers : INotificationHandler<InvoiceCreated>, INotificationHandler<LineItemAdded>, 
        INotificationHandler<InvoiceBalanceUpdated>, INotificationHandler<ReportPrinted>
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IGuidGenerator _GuidGenerator;
        private readonly Stream _InvoiceStream;

        public UpdateInvoiceReadModelEventHandlers(IMediator mediator, Store store, IUnitOfWork unitOfWork, IGuidGenerator guidGenerator)
        {
            _Mediator = mediator;
            _UnitOfWork = unitOfWork;
            _GuidGenerator = guidGenerator;
            _InvoiceStream = store.Open(Streams.Invoices);

            _Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReportPrinted, InvoicePrinted>()
                    .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.RecordId));
            }).CreateMapper();
        }

        public void Handle(InvoiceCreated notification)
        {
            var invoice = new Invoice();
            notification.Apply(invoice);

            const string query =
                "INSERT INTO Accounting.Invoice (Id, Balance, CreatedById, InvoiceNumber, CompanyId, CreatedOn) " +
                "VALUES (@Id, @Balance, @CreatedById, @InvoiceNumber, @CompanyId, @CreatedOn)";

            _UnitOfWork.Execute(query, new
            {
                invoice.Id,
                invoice.Balance,
                CreatedById = invoice.CreatedBy.Id,
                invoice.InvoiceNumber,
                CompanyId = invoice.Company.Id,
                CreatedOn = notification.EventDate
            });
        }

        public void Handle(LineItemAdded notification)
        {
            var lineItem = new LineItem();
            notification.Apply(lineItem);

            const string query = @"
                INSERT INTO Accounting.LineItem (Id, InvoiceId, Description, Amount, IsPaid, CreatedOn, CreatedById)
                VALUES (@Id, @InvoiceId, @Description, @Amount, @IsPaid, @CreatedOn, @CreatedById)";

            _UnitOfWork.Execute(query, new
            {
                lineItem.Id,
                lineItem.InvoiceId,
                lineItem.Description,
                lineItem.Amount,
                lineItem.IsPaid,
                CreatedOn = notification.EventDate,
                CreatedById = lineItem.CreatedBy.Id
            });

        }

        public void Handle(InvoiceBalanceUpdated notification)
        {
            var invoice = _Mediator.Send(new GetInvoice {Id = notification.InvoiceId});
            notification.Apply(invoice);

            const string query = @"
                UPDATE Accounting.Invoice
                SET Balance = @Balance
                WHERE Id = @Id";

            _UnitOfWork.Execute(query, invoice);

            // This assumes a liability account, I'm considering invoices to 
            // be liabilities for the company that receives an invoice
            var ledgerEntry = new GeneralLedgerEntry
            {
                CreditAmount = notification.Amount > 0 ? notification.Amount : 0,
                DebitAmount = notification.Amount < 0 ? notification.Amount : 0,
                EntryDate = notification.EventDate,
                Id = _GuidGenerator.Generate(),
                LineItemId = notification.LineItemId,
                CreatedOn = notification.EventDate,
                CreatedById = notification.UpdatedById
            };

            const string query2 = @"
                INSERT INTO Accounting.GeneralLedger (Id, CreditAmount, DebitAmount, LineItemId, CreatedOn, CreatedById)
                VALUES (@Id, @CreditAmount, @DebitAmount, @LineItemId, @CreatedOn, @CreatedById)";

            _UnitOfWork.Execute(query2, ledgerEntry);
        }

        public void Handle(ReportPrinted notification)
        {
            if (notification.TableId != Table.InvoiceTableId)
            {
                // We're only interested in invoice reports, other reports
                // we could not care less about
                return;
            }

            _InvoiceStream.Write<InvoicePrinted>(builder => builder
                .WithCorrelationId(notification.RecordId)
                .WithEvent(_Mapper.Map<ReportPrinted, InvoicePrinted>(notification))
                .WithMetaData(evt => new { evt.ReportId }));
        }
    }
}
