using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Events;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Events.Payments;

namespace InvoiceCqrs.Visitors
{
    public interface IInvoiceEventVisitor
    {
        IList<EventHistoryItem> EventHistory { get; }

        void Visit(InvoiceCreated evt);

        void Visit(InvoiceBalanceUpdated evt);

        void Visit(LineItemAdded evt);

        void Visit(LineItemPaid evt);

        void Visit(PaymentApplied evt);

        void Visit(PaymentUnapplied evt);

        IList<EventHistoryItem> Visit(IList<IVisitable<IInvoiceEventVisitor>> events);
    }
}
