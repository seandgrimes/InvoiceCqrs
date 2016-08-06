using System;
using System.Linq;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Messages.Commands.Companies;
using InvoiceCqrs.Messages.Commands.Invoices;
using InvoiceCqrs.Messages.Commands.Payments;
using InvoiceCqrs.Messages.Commands.Reports;
using InvoiceCqrs.Messages.Commands.Users;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Util;
using MediatR;

namespace InvoiceCqrs
{
    public class Application
    {
        private readonly IMediator _Mediator;
        private readonly IGuidGenerator _GuidGenerator;

        public Application(IMediator mediator, IGuidGenerator guidGenerator)
        {
            _Mediator = mediator;
            _GuidGenerator = guidGenerator;
        }

        public void Run()
        {
            var companyId = _GuidGenerator.Generate();
            var company = _Mediator.Send(new CreateCompany
            {
                Addr1 = "123 Main Street",
                Addr2 = string.Empty,
                City = "Shreveport",
                CompanyId = companyId,
                Name = $"Test Company {companyId}",
                State = "LA",
                ZipCode = "71101"
            });

            var userId = _GuidGenerator.Generate();
            var user = _Mediator.Send(new CreateUser
            {
                Email = $"{userId}@example.com",
                FirstName = "Test",
                Id = userId,
                LastName = "User"
            });

            var invoice = _Mediator.Send(new CreateInvoice
            {
                CompanyId = company.Id,
                CreatedById = user.Id,
                Id = _GuidGenerator.Generate(),
                InvoiceNumber = Guid.NewGuid().ToString("N").Substring(0, 10)
            });

            var lineItem1 = _Mediator.Send(new AddLineItem
            {
                Id = _GuidGenerator.Generate(),
                InvoiceId = invoice.Id,
                Amount = 25,
                Description = "Fee 1",
                CreatedById = user.Id
            });

            var lineItem2 = _Mediator.Send(new AddLineItem
            {
                Id = _GuidGenerator.Generate(),
                InvoiceId = invoice.Id,
                Amount = 50,
                Description = "Fee 2",
                CreatedById = user.Id
            });

            var payment = _Mediator.Send(new ReceivePayment
            {
                Id = _GuidGenerator.Generate(),
                Amount = 100,
                ReceivedOn = DateTime.Now,
                ReceivedById = user.Id
            });

            _Mediator.Send(new ApplyPayment
            {
                Id = _GuidGenerator.Generate(),
                Amount = 25,
                LineItemId = lineItem1.Id,
                PaymentId = payment.Id,
                AppliedById = user.Id
            });

            _Mediator.Send(new UnapplyPayment
            {
                Amount = 25,
                LineItemId = lineItem1.Id,
                PaymentId = payment.Id,
                UnappliedById = user.Id
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

            var readModelInvoice = _Mediator.Send(new GetInvoice {Id = invoice.Id});

            Console.WriteLine(readModelInvoice);
            invoiceHistory.ToList().ForEach(history => Console.WriteLine($"{history.EventDate}: {history.Message}"));
        }
    }
}
