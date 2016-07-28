using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Domain
{
    public class Payment : Entity
    {
        public DateTime ReceivedOn { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public void ApplyTo(LineItem lineItem)
        {
            lineItem.IsPaid = true;
            Balance -= lineItem.Amount;
        }
    }
}
