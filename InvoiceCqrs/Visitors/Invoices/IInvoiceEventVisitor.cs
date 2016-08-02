using System.Collections.Generic;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Events.Payments;

namespace InvoiceCqrs.Visitors.Invoices
{
    public interface IInvoiceEventVisitor
    {
        IList<EventHistoryItem> EventHistory { get; }

        void Visit(InvoiceCreated evt);

        void Visit(InvoiceBalanceUpdated evt);

        void Visit(InvoicePrinted evt);

        void Visit(LineItemAdded evt);

        void Visit(LineItemPaid evt);

        void Visit(PaymentApplied evt);

        void Visit(PaymentUnapplied evt);

        IList<EventHistoryItem> Visit(IList<IVisitable<IInvoiceEventVisitor>> events);
    }
}
