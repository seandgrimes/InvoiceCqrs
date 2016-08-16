using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Commands.Invoices;
using InvoiceCqrs.Messages.Queries.Companies;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Util;
using InvoiceCqrs.Web.Actions;
using InvoiceCqrs.Web.Models;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InvoiceCqrs.Web.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly IGuidGenerator _GuidGenerator;

        public InvoicesController(IMediator mediator, IGuidGenerator guidGenerator)
        {
            _Mediator = mediator;
            _GuidGenerator = guidGenerator;
            _Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<Invoice, InvoiceViewModel>()
                    .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => $"{src.Balance:C}"));
                cfg.CreateMap<LineItem, LineItemViewModel>()
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => $"{src.Amount:C}"));
                cfg.CreateMap<EventHistoryItem, EventHistoryItemViewModel>();
            }).CreateMapper();
        }

        [UnitOfWork]
        public ActionResult Add()
        {
            var viewModel = new AddEditInvoiceViewModel();
            viewModel.Companies = _Mediator.Send(new GetCompanies())
                .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString()})
                .ToList();

            var json = JsonConvert.SerializeObject(viewModel, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return View((object)json);
        }

        [HttpPost]
        [UnitOfWork]
        public ActionResult Add(AddEditInvoiceViewModel viewModel)
        {
            // This is bad, we probably need to implement some type of
            // login here in the future
            var createdBy = _Mediator.Send(new GetInvoices())
                .First()
                .CreatedBy;

            var invoice = _Mediator.Send(new CreateInvoice()
            {
                CompanyId = viewModel.CompanyId,
                CreatedById = createdBy.Id,
                Id = _GuidGenerator.Generate(),
                InvoiceNumber = viewModel.InvoiceNumber
            });

            foreach (var lineItem in viewModel.LineItems)
            {
                _Mediator.Send(new AddLineItem()
                {
                    Amount = Convert.ToDecimal(lineItem.Amount),
                    CreatedById = createdBy.Id,
                    Description = lineItem.Description,
                    Id = _GuidGenerator.Generate(),
                    InvoiceId = invoice.Id
                });

                throw new Exception("Testing rollback");
            }

            return Json(_Mapper.Map<InvoiceViewModel>(invoice));
        }

        // GET: Invoices
        [UnitOfWork]
        public ActionResult Index()
        {
            var invoices = _Mediator.Send(new GetInvoices());
            var viewModel = _Mapper.Map<IList<InvoiceViewModel>>(invoices);

            return View(viewModel);
        }

        [UnitOfWork]
        public ActionResult View(Guid id)
        {
            var invoice = _Mediator.Send(new GetInvoice {Id = id});
            var viewModel = _Mapper.Map<InvoiceViewModel>(invoice);

            var history = _Mediator.Send(new GetInvoiceHistory {InvoiceId = id});
            viewModel.History = _Mapper.Map<IList<EventHistoryItemViewModel>>(history);

            return View(viewModel);
        }
    }
}