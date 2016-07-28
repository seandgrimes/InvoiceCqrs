using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Domain
{
    public class LineItem : Entity
    {
        public Guid InvoiceId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool IsPaid { get; set; }
    }
}
