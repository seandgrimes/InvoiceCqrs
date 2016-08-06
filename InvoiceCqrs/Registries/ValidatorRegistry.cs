using InvoiceCqrs.Validation;
using StructureMap;

namespace InvoiceCqrs.Registries
{
    public class ValidatorRegistry : Registry
    {
        public ValidatorRegistry()
        {
            Scan(scanner =>
            {
                scanner.AssemblyContainingType(typeof(IValidator<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
            });
        }
    }
}
