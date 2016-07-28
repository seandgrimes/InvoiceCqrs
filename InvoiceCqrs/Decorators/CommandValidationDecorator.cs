using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Validation;
using MediatR;

namespace InvoiceCqrs.Decorators
{
    public class CommandValidationDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _Decorated;
        private readonly IList<IValidator<TRequest>> _Validators;

        public CommandValidationDecorator(IRequestHandler<TRequest, TResponse> decorated, IList<IValidator<TRequest>> validators)
        {
            _Decorated = decorated;
            _Validators = validators;
        }

        public TResponse Handle(TRequest message)
        {
            var validationErrors = _Validators.ToList()
                .SelectMany(validator => validator.Validate(message))
                .ToList();

            if (validationErrors.Any())
            {
                // We can do better than this...
                throw new CommandValidationException<TRequest>(message, validationErrors);
            }

            return _Decorated.Handle(message);
        }
    }
}
