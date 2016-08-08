using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceCqrs.Web.Models
{
    public class LineItemViewModel
    {
        public string Amount { get; set; }

        public string Description { get; set; }

        public UserViewModel CreatedBy { get; set; }
    }
}