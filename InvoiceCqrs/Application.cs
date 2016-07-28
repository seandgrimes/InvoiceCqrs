using System;
using System.Linq;
using InvoiceCqrs.EventStore;
using InvoiceCqrs.Messages.Commands;
using InvoiceCqrs.Messages.Events;
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

            var payment = stream.Write(invoice.Id, new PaymentReceived
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                ReceivedOn = DateTime.Now
            });

            stream.Write(invoice.Id, new PaymentApplied
            {
                Amount = 25,
                LineItemId = lineItem1.Id,
                PaymentId = payment.Id
            });

            var events = stream.Events
                .Where(evt => evt.ExternalId == invoice.Id)
                .Select(evt => JsonConvert.DeserializeObject(evt.JsonContent, evt.EventType) as IVisitable<IInvoiceEventVisitor>)
                .ToList();

            events.ForEach(evt => evt.Accept(_InvoiceVisitor));
                
            Console.WriteLine(ReadModel.Invoices[invoice.Id]);
            Console.WriteLine(string.Join("\n", _InvoiceVisitor.Messages));
        }
    }
}
