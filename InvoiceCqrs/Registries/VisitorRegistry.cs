using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Visitors.Invoices;
using StructureMap;

namespace InvoiceCqrs.Registries
{
    public class VisitorRegistry : Registry
    {
        public VisitorRegistry()
        {
            For<IInvoiceEventVisitor>().Use<InvoiceHistoryVisitor>();
        }
    }
}
