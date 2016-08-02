using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Messages.Events.Users
{
    public class UserCreated : IEvent
    {
        public string Email { get; set; }

        public DateTime EventDateTime { get; } = DateTime.Now;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid UserId { get; set; }
    }
}
