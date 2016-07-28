using InvoiceCqrs.Decorators;
using InvoiceCqrs.EventStore;
using InvoiceCqrs.Validation;
using InvoiceCqrs.Visitors;
using MediatR;
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
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));

                    scanner.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
                });

                cfg.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
                cfg.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                cfg.For<IMediator>().Use<Mediator>();

                cfg.For<Store>().Use<Store>().Singleton();
                cfg.For<Application>().Use<Application>().Singleton();
                cfg.For<IInvoiceEventVisitor>().Use<InvoiceHistoryVisitor>();

                cfg.For(typeof(IRequestHandler<,>)).DecorateAllWith(typeof(CommandValidationDecorator<,>));
            });

            var app = container.GetInstance<Application>();
            app.Run();
        }
    }
}