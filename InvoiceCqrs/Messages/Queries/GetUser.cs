using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Queries
{
    public class GetUser : IRequest<User>
    {
        public Guid UserId { get; set; }
    }
}
