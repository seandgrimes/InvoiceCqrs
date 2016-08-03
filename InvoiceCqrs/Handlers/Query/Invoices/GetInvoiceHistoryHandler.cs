using System.Collections.Generic;
using System.Linq;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Queries;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence.EventStore;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;
using MediatR;
using Newtonsoft.Json;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoiceHistoryHandler : IRequestHandler<GetInvoiceHistory, IList<EventHistoryItem>>
    {
        private readonly IInvoiceEventVisitor _InvoiceVisitor;
        private readonly Stream _Stream;

        public GetInvoiceHistoryHandler(Store store, IInvoiceEventVisitor invoiceVisitor)
        {
            _InvoiceVisitor = invoiceVisitor;
            _Stream = store.Open(Streams.Invoices);
        }

        public IList<EventHistoryItem> Handle(GetInvoiceHistory message)
        {
            var events = _Stream.Events
                .Where(evt => evt.ExternalId == message.InvoiceId)
                .Select(evt => JsonConvert.DeserializeObject(evt.JsonContent, evt.EventType) as IVisitable<IInvoiceEventVisitor>)
                .Where(evt => evt != null)
                .ToList();

            _InvoiceVisitor.Visit(events);

            return _InvoiceVisitor.EventHistory;
        }
    }
}