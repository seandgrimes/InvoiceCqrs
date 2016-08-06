using AutoMapper;
using InvoiceCqrs.Messages.Commands.Payments;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Events.Payments;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Payments
{
    public class UnapplyPaymentHandler : IRequestHandler<UnapplyPayment, bool>
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public UnapplyPaymentHandler(IMediator mediator, Store store)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);

            _Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UnapplyPayment, PaymentUnapplied>();
                cfg.CreateMap<UnapplyPayment, InvoiceBalanceUpdated>()
                    .ForMember(dest => dest.UpdatedById, opt => opt.MapFrom(src => src.UnappliedById));
                cfg.CreateMap<UnapplyPayment, PaymentBalanceUpdated>()
                    .ForMember(dest => dest.UpdatedById, opt => opt.MapFrom(src => src.UnappliedById));
            }).CreateMapper();
        }

        public bool Handle(UnapplyPayment message)
        {
            var invoice = _Mediator.Send(new GetInvoiceForLineItem {LineItemId = message.LineItemId});

            var unapplied = _Stream.Write<PaymentUnapplied>(builder => builder
                .WithCorrelationId(message.PaymentId)
                .WithEvent(_Mapper.Map<PaymentUnapplied>(message))
                .WithMetaData(evt => new
                {
                    InvoiceId = invoice.Id,
                    evt.LineItemId
                }));

            var balanceUpdated = _Mapper.Map<InvoiceBalanceUpdated>(message);
            balanceUpdated.InvoiceId = invoice.Id;

            _Stream.Write<InvoiceBalanceUpdated>(builder => builder
                .WithCorrelationId(invoice.Id)
                .WithSourceEventId(unapplied.EventId)
                .WithEvent(balanceUpdated));

            _Stream.Write<PaymentBalanceUpdated>(builder => builder
                .WithCorrelationId(message.PaymentId)
                .WithSourceEventId(unapplied.EventId)
                .WithEvent(_Mapper.Map<PaymentBalanceUpdated>(message)));

            _Stream.Write<PaymentBalanceUpdated>(builder => builder
                .WithCorrelationId(message.PaymentId)
                .WithSourceEventId(unapplied.EventId)
                .WithEvent(new PaymentBalanceUpdated
                {
                    Amount = message.Amount,
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