using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Util
{
    public interface IGuidGenerator
    {
        Guid Generate();
    }
}
