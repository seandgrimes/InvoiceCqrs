using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Domain.Entities
{
    public class Table : Entity
    {
        public static Guid InvoiceTableId = Guid.Parse("e130cdd6-72f0-40fb-96f9-c28dc55dc2ac");

        public string Name { get; set; }

        protected bool Equals(Table other)
        {
            return string.Equals(Name, other.Name);
        }
    }
}
