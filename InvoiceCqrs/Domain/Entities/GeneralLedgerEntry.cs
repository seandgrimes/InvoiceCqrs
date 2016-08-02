using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Domain.Entities
{
    public class GeneralLedgerEntry : Entity
    {
        public Guid LineItemId { get; set; }

        public decimal CreditAmount { get; set; }

        public decimal DebitAmount { get; set; }

        public DateTime EntryDate { get; set; }
    }
}
