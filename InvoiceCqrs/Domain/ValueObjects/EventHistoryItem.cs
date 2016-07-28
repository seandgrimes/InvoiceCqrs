using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Domain.ValueObjects
{
    public class EventHistoryItem
    {
        public DateTime EventDate { get; set; }

        public Type EventType { get; set; }

        public string Message { get; set; }
    }
}
