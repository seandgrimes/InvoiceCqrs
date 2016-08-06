using InvoiceCqrs.Util;
using StructureMap;

namespace InvoiceCqrs.Registries
{
    public class GeneratorRegistry : Registry
    {
        public GeneratorRegistry()
        {
            For<IGuidGenerator>().Use<SequentialGuidGenerator>();
        }
    }
}
