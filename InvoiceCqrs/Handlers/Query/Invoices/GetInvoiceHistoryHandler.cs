using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using InvoiceCqrs.Domain.ValueObjects;
using InvoiceCqrs.Messages.Queries.Invoices;
using InvoiceCqrs.Persistence;
using InvoiceCqrs.Persistence.EventStore.Util;
using InvoiceCqrs.Visitors;
using InvoiceCqrs.Visitors.Invoices;
using MediatR;

namespace InvoiceCqrs.Handlers.Query.Invoices
{
    public class GetInvoiceHistoryHandler : IRequestHandler<GetInvoiceHistory, IList<EventHistoryItem>>
    {
        private readonly IInvoiceEventVisitor _InvoiceVisitor;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IEventHydrator _EventHydrator;
        
        public GetInvoiceHistoryHandler(IInvoiceEventVisitor invoiceVisitor, IUnitOfWork unitOfWork, IEventHydrator eventHydrator)
        {
            _InvoiceVisitor = invoiceVisitor;
            _UnitOfWork = unitOfWork;
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

            var events = _UnitOfWork.Query<Domain.Entities.EventStore.Event>(eventsQuery, message).ToList();

            var visitable = _EventHydrator.Hydrate(events)
                .Select(evt => evt as IVisitable<IInvoiceEventVisitor, EventHistoryItem>)
                .Where(evt => evt != null)
                .ToList();

            return _InvoiceVisitor.Visit(visitable);
        }
    }
}