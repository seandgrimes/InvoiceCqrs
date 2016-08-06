using StructureMap;
using StructureMap.Graph;

namespace InvoiceCqrs
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.TheCallingAssembly();
                    scanner.LookForRegistries();
                });

                cfg.For<Application>().Use<Application>().Singleton();
            });

            var app = container.GetInstance<Application>();
            app.Run();
        }
    }
}