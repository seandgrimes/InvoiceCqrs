using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace InvoiceCqrs.Registries
{
    public class DatabaseRegistry : Registry
    {
        public DatabaseRegistry()
        {
            For<IDbConnection>().Singleton().Use(c => new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString));
        }
    }
}
