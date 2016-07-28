using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.EventStore
{
    public interface ISubscribe<in TEvent>
    {
        void Receive(TEvent evt);
    }
}
