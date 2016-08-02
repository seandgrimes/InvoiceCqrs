using System;
using InvoiceCqrs.Domain.Entities;
using InvoiceCqrs.Domain.ValueObjects;

namespace InvoiceCqrs.Messages.Events.Companies
{
    public class CompanyCreated : IEvent<Company>
    {
        public string Addr1 { get; set; }

        public string Addr2 { get; set; }

        public DateTime EventDateTime { get; }

        public string City { get; set; }

        public Guid CompanyId { get; set; }

        public string Name { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public void Apply(Company target)
        {
            target.Id = CompanyId;
            target.Name = Name;
            target.Address = new Address
            {
                Addr1 = Addr1,
                Addr2 = Addr2,
                City = City,
                State = State,
                ZipCode = ZipCode
            };
        }
    }
}
