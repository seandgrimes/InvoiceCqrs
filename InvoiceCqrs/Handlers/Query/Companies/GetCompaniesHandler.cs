using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Companies;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Companies
{
    public class GetCompaniesHandler : IRequestHandler<GetCompanies, IList<Company>>
    {
        private readonly IDbConnection _DbConnection;

        public GetCompaniesHandler(IDbConnection dbConnection)
        {
            _DbConnection = dbConnection;
        }

        public IList<Company> Handle(GetCompanies message)
        {
            const string query =
                @"  SELECT c.*
                    FROM Companies.Company c
                    ORDER BY c.Name ASC";

            return _DbConnection.Query<Company>(query)
                .ToList();
        }
    }
}
