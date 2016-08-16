using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using InvoiceCqrs.Persistence;
using StructureMap;
using StructureMap.Pipeline;

namespace InvoiceCqrs.Web.StructureMap.Registries
{
    public class DatabaseRegistry : Registry
    {
        public DatabaseRegistry()
        {
            For<IDbConnection>().Use(c => new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString))
                .LifecycleIs<UniquePerRequestLifecycle>();
            For<IUnitOfWork>().Use<UnitOfWork>().LifecycleIs<UniquePerRequestLifecycle>();

            // This is to populate the UnitOfWork property of UnitOfWorkAttribute
            Policies.FillAllPropertiesOfType<IUnitOfWork>().Use<UnitOfWork>();
        }
    }
}