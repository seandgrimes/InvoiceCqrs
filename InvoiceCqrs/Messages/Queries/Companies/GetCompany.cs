using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Queries.Companies
{
    public class GetCompany : IRequest<Company>
    {
        public Guid CompanyId { get; set; }
    }
}
