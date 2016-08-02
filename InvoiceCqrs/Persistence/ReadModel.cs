using System;
using System.Collections.Generic;
using InvoiceCqrs.Domain.Entities;

namespace InvoiceCqrs.Persistence
{
    public static class ReadModel
    {
        public static IDictionary<Guid, GeneralLedgerEntry> GeneralLedgerEntries { get; } = new Dictionary<Guid, GeneralLedgerEntry>();

        public static IDictionary<Guid, Invoice> Invoices { get; } = new Dictionary<Guid, Invoice>();

        public static IDictionary<Guid, LineItem> LineItems { get; } = new Dictionary<Guid, LineItem>();

        public static IDictionary<Guid, Payment> Payments { get; } = new Dictionary<Guid, Payment>();

        public static IDictionary<Guid, Table> Tables { get; } = new Dictionary<Guid, Table>();

        public static IDictionary<Guid, Report> Reports { get; } = new Dictionary<Guid, Report>();

        public static IDictionary<Guid, User> Users { get; } = new Dictionary<Guid, User>();

        static ReadModel()
        {
            var invoiceTable = new Table
            {
                Id = Guid.Parse("e130cdd6-72f0-40fb-96f9-c28dc55dc2ac"),
                Name = "tblInvoice"
            };
            Tables[invoiceTable.Id] = invoiceTable;

            var invoiceReport = new Report
            {
                Id = Guid.Parse("ed91e3cd-2f18-493d-9a8f-8ad47d780d52"),
                Name = "Invoice Report"
            };
            Reports[invoiceReport.Id] = invoiceReport;
        }
    }
}
