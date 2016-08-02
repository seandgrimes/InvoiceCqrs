using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Queries.Users;
using InvoiceCqrs.Messages.Shared;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Users
{
    public class SearchUsersHandler : IRequestHandler<SearchUsers, IList<User>>
    {
        public IList<User> Handle(SearchUsers message)
        {
            var comparisons = new List<Func<User, bool>>
            {
                u => message.Email == null || message.Email == u.Email,
                u => message.FirstName == null || message.FirstName == u.FirstName,
                u => message.UserId == default(Guid) || message.UserId == u.Id,
                u => message.LastName == null || message.LastName == u.LastName
            };

            
            if (message.SearchOption == SearchOptions.MatchAny)
            {
                return ReadModel.Users.Values
                    .Where(u => comparisons.Any(c => c(u)))
                    .ToList();
            }

            if (message.SearchOption == SearchOptions.MatchAll)
            {
                return ReadModel.Users.Values
                    .Where(u => comparisons.All(c => c(u)))
                    .ToList();
            }

            return new List<User>();
        }
    }
}
