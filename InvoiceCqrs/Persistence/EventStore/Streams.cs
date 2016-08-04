using System;

namespace InvoiceCqrs.Persistence.EventStore
{
    public static class Streams
    {
        public static readonly Guid Companies = Guid.Parse("902e2636-5a4f-11e6-a7ab-b8ca3a940de8");

        public static readonly Guid Invoices = Guid.Parse("902e2637-5a4f-11e6-a7ab-b8ca3a940de8");

        public static readonly Guid Reports = Guid.Parse("902e2638-5a4f-11e6-a7ab-b8ca3a940de8");

        public static readonly Guid Users = Guid.Parse("902e2639-5a4f-11e6-a7ab-b8ca3a940de8");
    }
}
