using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain;

namespace InvoiceCqrs
{
    public static class ReadModel
    {
        public static IDictionary<Guid, Invoice> Invoices { get; } = new Dictionary<Guid, Invoice>();

        public static IDictionary<Guid, LineItem> LineItems { get; } = new Dictionary<Guid, LineItem>();

        public static IDictionary<Guid, Payment> Payments { get; } = new Dictionary<Guid, Payment>();
    }
}
