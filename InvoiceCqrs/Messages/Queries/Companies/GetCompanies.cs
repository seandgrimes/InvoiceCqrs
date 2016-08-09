using System.Collections.Generic;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Queries.Companies
{
    public class GetCompanies : IRequest<IList<Company>>
    {
    }
}
