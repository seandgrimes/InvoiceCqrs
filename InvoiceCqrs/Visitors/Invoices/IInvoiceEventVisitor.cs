using System.Collections.Generic;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Events.Payments;

namespace InvoiceCqrs.Visitors.Invoices
{
    public interface IInvoiceEventVisitor
    {
        EventHistoryItem Visit(InvoiceCreated evt);

        EventHistoryItem Visit(InvoiceBalanceUpdated evt);

        EventHistoryItem Visit(InvoicePrinted evt);

        EventHistoryItem Visit(LineItemAdded evt);

        EventHistoryItem Visit(LineItemPaid evt);

        EventHistoryItem Visit(PaymentApplied evt);

        EventHistoryItem Visit(PaymentUnapplied evt);

        IList<EventHistoryItem> Visit(IList<IVisitable<IInvoiceEventVisitor, EventHistoryItem>> events);
    }
}
