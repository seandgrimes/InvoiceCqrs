using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Users;
using InvoiceCqrs.Messages.Shared;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Users
{
    public class SearchUsersHandler : IRequestHandler<SearchUsers, IList<User>>
    {
        private readonly IDbConnection _DbConnection;

        public SearchUsersHandler(IDbConnection dbConnection)
        {
            _DbConnection = dbConnection;
        }

        public IList<User> Handle(SearchUsers message)
        {
            // There a better way to do this?
            const string query =
                @"  SELECT u.Id, u.Email, u.FirstName, u.LastName
                    FROM Users.[User] u
                    WHERE 
                        (@Email IS NULL OR u.Email = @Email) {CompOper}
                        (@FirstName IS NULL OR u.FirstName = @FirstName) {CompOper}
                        (@UserIdIsDefaultValue = 1 OR u.Id = @UserId) {CompOper}
                        (@LastName IS NULL OR u.LastName = @LastName)";

            var operators = new Dictionary<SearchOptions, string>
            {
                {SearchOptions.MatchAny, "OR"},
                {SearchOptions.MatchAll, "AND"}
            };

            var results = _DbConnection.Query<User>(query.Replace("{CompOper}", operators[message.SearchOption]), new
            {
                message.Email,
                message.FirstName,
                message.LastName,
                UserIdIsDefaultValue = message.UserId == default(Guid) ? 1 : 0,
                message.UserId
            });

            return results.ToList();
        }
    }
}
