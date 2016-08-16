using System.Collections.Generic;
using System.Linq;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Companies;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Companies
{
    public class GetCompaniesHandler : IRequestHandler<GetCompanies, IList<Company>>
    {
        private readonly IUnitOfWork _UnitOfWork;

        public GetCompaniesHandler(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IList<Company> Handle(GetCompanies message)
        {
            const string query =
                @"  SELECT c.*
                    FROM Companies.Company c
                    ORDER BY c.Name ASC";

            return _UnitOfWork.Query<Company>(query)
                .ToList();
        }
    }
}
