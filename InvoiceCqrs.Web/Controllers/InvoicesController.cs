using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
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
        }

        // GET: Invoices
        public ActionResult Index()
        {
            return View();
        }
    }
}