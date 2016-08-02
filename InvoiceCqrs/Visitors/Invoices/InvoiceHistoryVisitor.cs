using System.Collections.Generic;
using System.Linq;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Events;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Events.Payments;
using InvoiceCqrs.Messages.Queries.Reports;
using InvoiceCqrs.Messages.Queries.Users;
using MediatR;

namespace InvoiceCqrs.Visitors.Invoices

{
    public class InvoiceHistoryVisitor : IInvoiceEventVisitor
    {
        public IList<EventHistoryItem> EventHistory { get; } = new List<EventHistoryItem>();

        private readonly Invoice _Invoice = new Invoice();

        private readonly IMediator _Mediator;

        public InvoiceHistoryVisitor(IMediator mediator)
        {
            _Mediator = mediator;
        }

        private static EventHistoryItem CreateEventHistoryItem(IEvent evt)
        {
            return new EventHistoryItem
            {
                EventDate = evt.EventDateTime,
                EventType = evt.GetType()
            };
        }

        public void Visit(InvoiceCreated evt)
        {
            evt.Apply(_Invoice);

            var historyItem = CreateEventHistoryItem(evt);
            historyItem.Message = "Invoice created";
            EventHistory.Add(historyItem);
        }

        public void Visit(InvoiceBalanceUpdated evt)
        {
            evt.Apply(_Invoice);

            var historyItem = CreateEventHistoryItem(evt);
            historyItem.Message = $"New balance of ${_Invoice.Balance}";
            EventHistory.Add(historyItem);
        }

        public void Visit(InvoicePrinted evt)
        {
            var report = _Mediator.Send(new GetReport {ReportId = evt.ReportId});
            var user = _Mediator.Send(new GetUser {UserId = evt.PrintedById});
            var historyItem = CreateEventHistoryItem(evt);
            
            // ReSharper disable once UseStringInterpolation
            historyItem.Message = string.Format("{0} - {1} By: {2} {3}",
                report.Name,
                evt.IsReprint ? "Reprint" : "Printed",
                user.FirstName,
                user.LastName);

            EventHistory.Add(historyItem);
        }

        public void Visit(LineItemAdded evt)
        {
            var lineItem = new LineItem();
            evt.Apply(lineItem);

            _Invoice.LineItems.Add(lineItem);

            var historyItem = CreateEventHistoryItem(evt);
            historyItem.Message = $"Line item {evt.Id} added to invoice {evt.InvoiceId}";
            EventHistory.Add(historyItem);
        }

        public void Visit(LineItemPaid evt)
        {
            var lineItem = _Invoice.LineItems.Single(li => li.Id == evt.LineItemId);
            evt.Apply(lineItem);

            var historyItem = CreateEventHistoryItem(evt);
            historyItem.Message = $"Line item {evt.LineItemId} has been paid";
            EventHistory.Add(historyItem);
        }

        public void Visit(PaymentApplied evt)
        {
            var historyItem = CreateEventHistoryItem(evt);
            historyItem.Message = $"Payment {evt.PaymentId} applied to line item {evt.LineItemId}";
            EventHistory.Add(historyItem);
        }

        public void Visit(PaymentUnapplied evt)
        {
            var historyItem = CreateEventHistoryItem(evt);
            historyItem.Message = $"Payment {evt.PaymentId} unapplied to line item {evt.LineItemId}";
            EventHistory.Add(historyItem);
        }

        public IList<EventHistoryItem> Visit(IList<IVisitable<IInvoiceEventVisitor>> events)
        {
            events.ToList().ForEach(evt => evt.Accept(this));
            return EventHistory;
        }
    }
}
