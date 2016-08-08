using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Web.Models;
using MediatR;

namespace InvoiceCqrs.Web.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;

        public InvoicesController(IMediator mediator)
        {
            _Mediator = mediator;
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

        // GET: Invoices
        public ActionResult Index()
        {
            var invoices = _Mediator.Send(new GetInvoices());
            var viewModel = _Mapper.Map<IList<InvoiceViewModel>>(invoices);

            return View(viewModel);
        }

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