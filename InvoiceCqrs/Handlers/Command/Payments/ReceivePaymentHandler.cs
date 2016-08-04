using AutoMapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands.Payments;
using InvoiceCqrs.Messages.Events.Payments;
using InvoiceCqrs.Messages.Queries.Payments;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Payments
{
    public class ReceivePaymentHandler : IRequestHandler<ReceivePayment, Payment>
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public ReceivePaymentHandler(Store store, IMediator mediator)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);

            _Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceivePayment, PaymentReceived>();

            }).CreateMapper();
        }

        public Payment Handle(ReceivePayment message)
        {
            //_Stream.Write(message.Id, new PaymentReceived
            //{
            //    Amount = message.Amount,
            //    Id = message.Id,
            //    ReceivedOn = message.ReceivedOn,
            //    ReceivedById = message.ReceivedById
            //});

            _Stream.Write<PaymentReceived>(builder => builder
                .WithCorrelationId(message.Id)
                .WithEvent(_Mapper.Map<PaymentReceived>(message)));
                
            return _Mediator.Send(new GetPayment {Id = message.Id});
        }
    }
}