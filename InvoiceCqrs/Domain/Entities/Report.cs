using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Domain.Entities
{
    public class Report : Entity
    {
        public static Guid InvoiceReportId = Guid.Parse("ed91e3cd-2f18-493d-9a8f-8ad47d780d52");

        public string Name { get; set; }
    }
}
