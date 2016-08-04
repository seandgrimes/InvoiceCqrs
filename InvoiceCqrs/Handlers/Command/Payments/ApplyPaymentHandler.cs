using AutoMapper;
using InvoiceCqrs.Messages.Commands.Payments;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Events.Payments;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Payments
{
    public class ApplyPaymentHandler : IRequestHandler<ApplyPayment, bool>
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;
        
        public ApplyPaymentHandler(Store store, IMediator mediator)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);

            _Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplyPayment, PaymentApplied>();
                cfg.CreateMap<ApplyPayment, LineItemPaid>();

            }).CreateMapper();
        }

        public bool Handle(ApplyPayment message)
        {
            var invoice = _Mediator.Send(new GetInvoiceForLineItem { LineItemId = message.LineItemId });

            _Stream.Write<PaymentApplied>(builder => builder
                .WithCorrelationId(message.PaymentId)
                .WithEvent(_Mapper.Map<PaymentApplied>(message))
                .WithMetaData(evt => new
                {
                    InvoiceId = invoice.Id,
                    evt.LineItemId
                }));

            _Stream.Write<LineItemPaid>(builder => builder
                .WithCorrelationId(message.LineItemId)
                .WithEvent(_Mapper.Map<LineItemPaid>(message))
                .WithMetaData(evt => new
                {
                    InvoiceId = invoice.Id
                }));

            _Stream.Write<InvoiceBalanceUpdated>(builder => builder
                .WithCorrelationId(invoice.Id)
                .WithEvent(new InvoiceBalanceUpdated
                {
                    Amount = -1*message.Amount,
                    InvoiceId = invoice.Id,
                    LineItemId = message.LineItemId,
                    PaymentId = message.PaymentId,
                    UpdatedById = message.AppliedById
                }));

            _Stream.Write<PaymentBalanceUpdated>(builder => builder
                .WithCorrelationId(message.PaymentId)
                .WithEvent(new PaymentBalanceUpdated
                {
                    Amount = -1*message.Amount,
                    PaymentId = message.PaymentId
                })
                .WithMetaData(evt => new
                {
                    message.LineItemId,
                    InvoiceId = invoice.Id
                }));

            return true;
        }
    }
}
