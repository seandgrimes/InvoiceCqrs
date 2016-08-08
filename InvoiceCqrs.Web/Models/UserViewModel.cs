using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceCqrs.Web.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}