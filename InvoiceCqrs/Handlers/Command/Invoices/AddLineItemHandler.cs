using AutoMapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands.Invoices;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Invoices
{
    public class AddLineItemHandler : IRequestHandler<AddLineItem, LineItem>
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public AddLineItemHandler(IMediator mediator, Store store)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);

            _Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddLineItem, LineItemAdded>();
                cfg.CreateMap<AddLineItem, InvoiceBalanceUpdated>()
                    .ForMember(dest => dest.LineItemId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.UpdatedById, opt => opt.MapFrom(src => src.CreatedById));
            }).CreateMapper();
        }

        public LineItem Handle(AddLineItem message)
        {
            _Stream.Write<LineItemAdded>(builder => builder
                .WithCorrelationId(message.Id)
                .WithEvent(_Mapper.Map<LineItemAdded>(message))
                .WithMetaData(evt => new
                {
                    evt.InvoiceId
                }));

            _Stream.Write<InvoiceBalanceUpdated>(builder => builder
                .WithCorrelationId(message.InvoiceId)
                .WithEvent(_Mapper.Map<InvoiceBalanceUpdated>(message))
                .WithMetaData(evt => new
                {
                    evt.InvoiceId
                }));

            return _Mediator.Send(new GetLineItem
            {
                Id = message.Id
            });
        }
    }
}
