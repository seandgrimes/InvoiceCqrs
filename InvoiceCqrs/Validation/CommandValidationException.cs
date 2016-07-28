using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Validation
{
    /// <summary>
    /// Exception with list of validation errors for a command that has failed
    /// validation
    /// </summary>
    /// <remarks>
    /// Validation errors aren't truly exceptional, they're often expected.
    /// Need to come up with a better pattern than this
    /// </remarks>
    public class CommandValidationException<TCommand> : Exception
    {
        public TCommand Command { get; private set; }

        public IList<string> ValidationErrors { get; private set; }

        public CommandValidationException(string message, TCommand command, IList<string> validationErrors) : base(message)
        {
            Command = command;
            ValidationErrors = validationErrors;
        }

        public CommandValidationException(TCommand command, IList<string> validationErrors)
            : base("Validation for the command failed")
        {
            Command = command;
            ValidationErrors = validationErrors;
        }
    }
}
