using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace InvoiceCqrs.Messages.Events
{
    public interface IEvent<in TTarget> : INotification
    {
        DateTime EventDateTime { get; }

        void Apply(TTarget target);
    }
}
