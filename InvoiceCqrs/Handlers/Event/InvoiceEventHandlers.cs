using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Events.Invoices;
using InvoiceCqrs.Messages.Events.Payments;
using InvoiceCqrs.Messages.Events.Reports;
using InvoiceCqrs.Persistence;
using InvoiceCqrs.Persistence.EventStore;
using MediatR;

namespace InvoiceCqrs.Handlers.Event
{
    public class InvoiceEventHandlers : INotificationHandler<InvoiceCreated>, INotificationHandler<LineItemAdded>, INotificationHandler<PaymentApplied>,
        INotificationHandler<PaymentUnapplied>, INotificationHandler<PaymentReceived>, INotificationHandler<InvoiceBalanceUpdated>,
        INotificationHandler<ReportPrinted>
    {
        private readonly IMediator _Mediator;
        private readonly Stream _InvoiceStream;

        public InvoiceEventHandlers(IMediator mediator, Store store)
        {
            _Mediator = mediator;
            _InvoiceStream = store.Open(Streams.Invoices);
        }

        public void Handle(InvoiceCreated notification)
        {
            if (ReadModel.Invoices.ContainsKey(notification.Id))
            {
                throw new Exception($"Invoice with external ID of {notification.Id} already exists");
            }

            var invoice = new Invoice();
            notification.Apply(invoice);
            ReadModel.Invoices[invoice.Id] = invoice;
        }

        public void Handle(LineItemAdded notification)
        {
            if (!ReadModel.Invoices.ContainsKey(notification.InvoiceId))
            {
                throw new Exception($"Invoice {notification.InvoiceId} does not exist");
            }

            var lineItem = new LineItem();
            notification.Apply(lineItem);

            var invoice = ReadModel.Invoices[notification.InvoiceId];
            invoice.LineItems.Add(lineItem);

            ReadModel.LineItems[notification.Id] = lineItem;
        }

        public void Handle(PaymentApplied notification)
        {
            // If we needed to maintain a read-model version of the payment history
            // for an invoice, we'd probably do something here
        }

        public void Handle(PaymentUnapplied notification)
        {
            // If wee needed to maintain a read-model version of the payment history
            // for an invoice, we'd probably do something here
        }

        public void Handle(PaymentReceived notification)
        {
            if (ReadModel.Payments.ContainsKey(notification.Id))
            {
                throw new Exception($"Payment with id {notification.Id} has already been received");
            }

            var payment = new Payment();
            notification.Apply(payment);

            ReadModel.Payments[notification.Id] = payment;
        }

        public void Handle(InvoiceBalanceUpdated notification)
        {
            var invoice = ReadModel.Invoices[notification.InvoiceId];
            notification.Apply(invoice);

            // This assumes a liability account, I'm considering invoices to 
            // be liabilities for the company that receives an invoice
            var ledgerEntry = new GeneralLedgerEntry
            {
                CreditAmount = notification.Amount > 0 ? notification.Amount : 0,
                DebitAmount = notification.Amount < 0 ? notification.Amount : 0,
                EntryDate = notification.EventDateTime,
                Id = Guid.NewGuid(),
                LineItemId = notification.LineItemId
            };

            ReadModel.GeneralLedgerEntries[ledgerEntry.Id] = ledgerEntry;
        }

        public void Handle(ReportPrinted notification)
        {
            if (notification.TableId != Table.InvoiceTableId)
            {
                // We're only interested in invoice reports, other reports
                // we could not care less about
                return;
            }

            _InvoiceStream.Write(notification.RecordId, new InvoicePrinted
            {
                InvoiceId = notification.RecordId,
                IsReprint = notification.IsReprint,
                PrintedById = notification.PrintedById,
                ReportId = notification.ReportId
            });
        }
    }
}
