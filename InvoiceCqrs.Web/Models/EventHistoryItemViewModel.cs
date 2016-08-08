using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceCqrs.Web.Models
{
    public class EventHistoryItemViewModel
    {
        public DateTime EventDate { get; set; }

        public string Message { get; set; }
    }
}