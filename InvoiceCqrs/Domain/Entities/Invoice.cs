using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvoiceCqrs.Domain.Entities
{
    public class Invoice : Entity
    {
        public decimal Balance { get; set; }

        public Company Company { get; set; }

        public User CreatedBy { get; set; }

        public string InvoiceNumber { get; set; }

        public IList<LineItem> LineItems { get; set; } = new List<LineItem>();

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