using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceCqrs.Messages.Commands.Users;
using InvoiceCqrs.Messages.Queries.Users;
using InvoiceCqrs.Messages.Shared;
using MediatR;

namespace InvoiceCqrs.Validation.Commands.Users
{
    public class CreateUserCommandValidator : IValidator<CreateUser>
    {
        private readonly IMediator _Mediator;

        public CreateUserCommandValidator(IMediator mediator)
        {
            _Mediator = mediator;
        }

        public IList<string> Validate(CreateUser entity)
        {
            var errors = new List<string>();

            var users = _Mediator.Send(new SearchUsers
            {
                Email = entity.Email,
                SearchOption = SearchOptions.MatchAny,
                UserId = entity.Id
            });

            if (users.Any())
            {
                errors.Add("A user with this email or ID already exists");
            }

            return errors;
        }
    }
}
