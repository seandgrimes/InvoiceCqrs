using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Domain
{
    public class Invoice : Entity
    {
        public decimal Balance { get; set; }

        public string InvoiceNumber { get; set; }

        public IList<LineItem> LineItems { get; set; } = new List<LineItem>();

        public void AddLineItem(LineItem lineItem)
        {
            LineItems.Add(lineItem);
            Balance += lineItem.Amount;
        }

        public void ApplyPayment(Payment payment, Guid lineItemId)
        {
            var lineItem = LineItems.SingleOrDefault(li => li.Id == lineItemId);
            if (lineItem == null)
            {
                throw new Exception($"Line item with ID of {lineItemId} does not exist");
            }

            payment.ApplyTo(lineItem);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Invoice ID: {Id}");
            builder.AppendLine($"Invoice Number: {InvoiceNumber}");
            builder.AppendLine($"Invoice Balance: ${Balance}");
            builder.AppendLine("-------------------------------------------------");

            for (var idx = 0; idx < LineItems.Count; idx++)
            {
                var lineItem = LineItems[idx];
                builder.AppendLine($"Line Item {idx + 1}: {lineItem.Description} - ${lineItem.Amount}");
            }

            return builder.ToString();
        }
    }
}