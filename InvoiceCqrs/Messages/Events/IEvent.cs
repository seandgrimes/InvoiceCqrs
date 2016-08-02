using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace InvoiceCqrs.Messages.Events
{
    public interface IEvent : INotification
    {
        DateTime EventDateTime { get; }
    }

    public interface IEvent<in TTarget> : IEvent
    {
        void Apply(TTarget target);
    }
}
