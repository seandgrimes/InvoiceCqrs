using System;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Queries.Users
{
    public class GetUser : IRequest<User>
    {
        public Guid UserId { get; set; }
    }
}
