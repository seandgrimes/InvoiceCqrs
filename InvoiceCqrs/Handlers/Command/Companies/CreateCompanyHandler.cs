using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands.Companies;
using InvoiceCqrs.Messages.Events.Companies;
using InvoiceCqrs.Messages.Queries.Companies;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Command.Companies
{
    public class CreateCompanyHandler : IRequestHandler<CreateCompany, Company>
    {
        private readonly Stream _EventStream;
        private readonly IMediator _Mediator;

        public CreateCompanyHandler(Store store, IMediator mediator)
        {
            _Mediator = mediator;
            _EventStream = store.Open(Streams.Companies);
        }

        public Company Handle(CreateCompany message)
        {
            _EventStream.Write<CompanyCreated>(builder => builder
                .WithCorrelationId(message.CompanyId)
                .WithEvent(new CompanyCreated
                {
                    Addr1 = message.Addr1,
                    Addr2 = message.Addr2,
                    City = message.City,
                    CompanyId = message.CompanyId,
                    Name = message.Name,
                    State = message.State,
                    ZipCode = message.ZipCode
                })
                .WithMetaData(evt => new
                {
                    evt.CompanyId
                }));

            return _Mediator.Send(new GetCompany {CompanyId = message.CompanyId});
        }
    }
}
