using System;
using System.Linq;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands.Invoices;
using InvoiceCqrs.Messages.Commands.Payments;
using InvoiceCqrs.Messages.Commands.Reports;
using InvoiceCqrs.Messages.Commands.Users;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using MediatR;

namespace InvoiceCqrs
{
    public class Application
    {
        private readonly IMediator _Mediator;
        
        public Application(IMediator mediator)
        {
            _Mediator = mediator;
        }

        public void Run()
        {
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

            var user = _Mediator.Send(new CreateUser
            {
                Email = "test.user@example.com",
                FirstName = "Test",
                Id = Guid.NewGuid(),
                LastName = "User"
            });

            _Mediator.Send(new PrintReport
            {
                PrintedById = user.Id,
                RecordId = invoice.Id,
                ReportId = Report.InvoiceReportId,
                TableId = Table.InvoiceTableId
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
