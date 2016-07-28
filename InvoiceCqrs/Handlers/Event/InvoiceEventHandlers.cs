using System;
using InvoiceCqrs.Domain;
using InvoiceCqrs.Messages.Events;
using InvoiceCqrs.Messages.Queries;
using MediatR;

namespace InvoiceCqrs.Handlers.Event
{
    public class InvoiceEventHandlers : INotificationHandler<InvoiceCreated>, INotificationHandler<LineItemAdded>, INotificationHandler<PaymentApplied>,
        INotificationHandler<PaymentUnapplied>, INotificationHandler<PaymentReceived>
    {
        private readonly IMediator _Mediator;

        public InvoiceEventHandlers(IMediator mediator)
        {
            _Mediator = mediator;
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
            invoice.AddLineItem(lineItem);

            ReadModel.LineItems[notification.Id] = lineItem;
        }

        public void Handle(PaymentApplied notification)
        {
            var invoice = _Mediator.Send(new GetInvoiceForLineItem {LineItemId = notification.LineItemId});
            var payment = ReadModel.Payments[notification.PaymentId];

            invoice.ApplyPayment(payment, notification.LineItemId);
        }

        public void Handle(PaymentUnapplied notification)
        {
            throw new NotImplementedException();
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
    }
}
