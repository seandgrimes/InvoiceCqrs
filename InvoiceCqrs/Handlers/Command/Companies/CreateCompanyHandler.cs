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
        private readonly IMediator _Mediator;
        private readonly Stream _Stream;

        public CreateCompanyHandler(Store store, IMediator mediator)
        {
            _Mediator = mediator;
            _Stream = store.Open(Streams.Companies);
        }

        public Company Handle(CreateCompany message)
        {
            _Stream.Write(message.CompanyId, new CompanyCreated
            {
                Addr1 = message.Addr1,
                Addr2 = message.Addr2,
                City = message.City,
                CompanyId = message.CompanyId,
                Name = message.Name,
                State = message.State,
                ZipCode = message.ZipCode
            });

            return _Mediator.Send(new GetCompany {CompanyId = message.CompanyId});
        }
    }
}
