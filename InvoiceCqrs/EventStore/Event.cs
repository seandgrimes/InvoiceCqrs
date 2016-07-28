using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.EventStore
{
    public class Event
    {
        public Type EventType { get; set; }

        public Guid ExternalId { get; set; }

        public string JsonContent { get; set; }

        
    }
}
