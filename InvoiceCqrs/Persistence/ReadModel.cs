using System;
using System.Collections.Generic;
using InvoiceCqrs.Domain.Entities;

namespace InvoiceCqrs.Persistence
{
    public static class ReadModel
    {
        public static IDictionary<Guid, Invoice> Invoices { get; } = new Dictionary<Guid, Invoice>();

        public static IDictionary<Guid, LineItem> LineItems { get; } = new Dictionary<Guid, LineItem>();

        public static IDictionary<Guid, Payment> Payments { get; } = new Dictionary<Guid, Payment>();
    }
}
