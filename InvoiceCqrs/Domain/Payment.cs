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
            if (lineItem.Amount > Balance)
            {
                throw new Exception($"There is an insufficient balance to apply payment {Id} to line item {lineItem.Id}");
            }

            lineItem.IsPaid = true;
            Balance -= lineItem.Amount;
        }
    }
}
