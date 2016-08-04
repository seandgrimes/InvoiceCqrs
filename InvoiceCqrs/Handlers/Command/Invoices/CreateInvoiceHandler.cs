using AutoMapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands.Invoices;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Invoices
{
    public class CreateInvoiceHandler : IRequestHandler<CreateInvoice, Invoice>
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public CreateInvoiceHandler(IMediator mediator, Store store)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Invoices);

            _Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateInvoice, InvoiceCreated>();

            }).CreateMapper();
        }

        public Invoice Handle(CreateInvoice message)
        {
            _Stream.Write<InvoiceCreated>(builder => builder
                .WithCorrelationId(message.Id)
                .WithEvent(_Mapper.Map<InvoiceCreated>(message))
                .WithMetaData(evt => new
                {
                    InvoiceId = evt.Id
                }));

            return _Mediator.Send(new GetInvoice
            {
                Id = message.Id
            });
        }
    }
}