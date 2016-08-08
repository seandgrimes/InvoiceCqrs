using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceCqrs.Web.Models
{
    public class InvoiceViewModel
    {
        public string Balance { get; set; }

        public string CompanyName { get; set; }

        public UserViewModel CreatedBy { get; set; }

        public IList<EventHistoryItemViewModel> History { get; set; } = new List<EventHistoryItemViewModel>();

        public Guid Id { get; set; }

        public string InvoiceNumber { get; set; }

        public IList<LineItemViewModel> LineItems { get; set; } = new List<LineItemViewModel>();
    }
}