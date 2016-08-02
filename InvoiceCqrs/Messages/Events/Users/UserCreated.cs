using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Messages.Events.Users
{
    public class UserCreated : IEvent
    {
        public Guid UserId { get; set; }

        public DateTime EventDateTime { get; } = DateTime.Now;
    }
}
