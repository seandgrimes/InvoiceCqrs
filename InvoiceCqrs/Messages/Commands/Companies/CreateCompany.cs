using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Domain.Entities;
using MediatR;

namespace InvoiceCqrs.Messages.Commands.Companies
{
    public class CreateCompany : IRequest<Company>
    {
        public Guid CompanyId { get; set; }

        public string Name { get; set; }

        public string Addr1 { get; set; }

        public string Addr2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }
    }
}
