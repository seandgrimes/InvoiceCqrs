using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceCqrs.Messages.Events;

namespace InvoiceCqrs.Visitors
{
    public interface IInvoiceEventVisitor
    {
        IList<string> Messages { get; set; }

        void Visit(InvoiceCreated evt);
        void Visit(LineItemAdded evt);
        void Visit(PaymentApplied evt);
       void Visit(PaymentUnapplied evt);
    }
}
