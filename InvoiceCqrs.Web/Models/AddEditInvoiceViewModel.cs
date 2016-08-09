using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceCqrs.Web.Models
{
    public class AddEditInvoiceViewModel : InvoiceViewModel
    {
        public Guid CompanyId { get; set; }

        public IList<SelectListItem> Companies { get; set; } = new List<SelectListItem>();
    }
}