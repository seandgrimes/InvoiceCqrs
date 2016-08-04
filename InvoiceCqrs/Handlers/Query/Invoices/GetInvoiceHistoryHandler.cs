using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence.EventStore.Util;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoiceHistoryHandler : IRequestHandler<GetInvoiceHistory, IList<EventHistoryItem>>
    {
        private readonly IInvoiceEventVisitor _InvoiceVisitor;
        private readonly IDbConnection _DbConnection;
        private readonly IEventHydrator _EventHydrator;
        
        public GetInvoiceHistoryHandler(IInvoiceEventVisitor invoiceVisitor, IDbConnection dbConnection, IEventHydrator eventHydrator)
        {
            _InvoiceVisitor = invoiceVisitor;
            _DbConnection = dbConnection;
            _EventHydrator = eventHydrator;
        }

        public IList<EventHistoryItem> Handle(GetInvoiceHistory message)
        {
            const string eventsQuery =
                @"  SELECT e.Id, e.CorrelationId, e.EventDate, e.EventType, e.IsDispatched, e.Json, e.StreamId
                    FROM EventStore.Event e
                    LEFT JOIN EventStore.EventMetadata emd ON e.Id = emd.EventId
                    WHERE e.CorrelationId = @InvoiceId
                        OR (emd.Name = 'InvoiceId' AND emd.Value = @InvoiceId)
                    ORDER BY e.EventDate;";

            var events = _DbConnection.Query<Domain.Entities.EventStore.Event>(eventsQuery, message).ToList();

            var visitable = _EventHydrator.Hydrate(events)
                .Select(evt => evt as IVisitable<IInvoiceEventVisitor>)
                .Where(evt => evt != null)
                .ToList();

            _InvoiceVisitor.Visit(visitable);

            return _InvoiceVisitor.EventHistory;
        }
    }
}