using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Shared;
using MediatR;

namespace InvoiceCqrs.Messages.Queries.Users
{
    public class SearchUsers : IRequest<IList<User>>
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public Guid UserId { get; set; }

        public string LastName { get; set; }

        public SearchOptions SearchOption { get; set; }
    }
}
