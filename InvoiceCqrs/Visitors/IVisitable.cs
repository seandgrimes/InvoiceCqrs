using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Visitors
{
    public interface IVisitable<in TVisitor>
    {
        void Accept(TVisitor visitor);
    }
}
