using System;
using System.Linq;
using InvoiceCqrs.Messages.Commands;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Persistence;
using InvoiceCqrs.Persistence.EventStore;
using InvoiceCqrs.Visitors;
using MediatR;
using Newtonsoft.Json;

namespace InvoiceCqrs
{
    public class Application
    {
        private readonly IMediator _Mediator;
        private readonly Store _Store;
        private readonly IInvoiceEventVisitor _InvoiceVisitor;

        public Application(IMediator mediator, Store store, IInvoiceEventVisitor invoiceVisitor)
        {
            _Mediator = mediator;
            _Store = store;
            _InvoiceVisitor = invoiceVisitor;
        }

        public void Run()
        {
            var stream = _Store.Open("invoices");

            var invoice = _Mediator.Send(new CreateInvoice
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = Guid.NewGuid().ToString("N").Substring(10)
            });

            var lineItem1 = _Mediator.Send(new AddLineItem
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                Amount = 25,
                Description = "Fee 1"
            });

            var lineItem2 = _Mediator.Send(new AddLineItem
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                Amount = 50,
                Description = "Fee 2"
            });

            var payment = _Mediator.Send(new ReceivePayment
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                ReceivedOn = DateTime.Now
            });

            _Mediator.Send(new ApplyPayment
            {
                Id = Guid.NewGuid(),
                Amount = 25,
                LineItemId = lineItem1.Id,
                PaymentId = payment.Id
            });

            _Mediator.Send(new UnapplyPayment
            {
                Amount = 25,
                LineItemId = lineItem1.Id,
                PaymentId = payment.Id
            });

            var invoiceHistory = _Mediator.Send(new GetInvoiceHistory
            {
                InvoiceId = invoice.Id
            });

            Console.WriteLine(ReadModel.Invoices[invoice.Id]);
            invoiceHistory.ToList().ForEach(history => Console.WriteLine($"{history.EventDate}: {history.Message}"));
        }
    }
}
