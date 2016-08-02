using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain.ValueObjects;

namespace InvoiceCqrs.Domain.Entities
{
    public class Company : Entity
    {
        public Address Address { get; set; }

        public string Name { get; set; }
    }
}
