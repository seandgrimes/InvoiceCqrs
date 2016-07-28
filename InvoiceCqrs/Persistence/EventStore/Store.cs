using System.Collections.Generic;
using MediatR;

namespace InvoiceCqrs.Persistence.EventStore
{
    public class Store
    {
        private readonly IMediator _Mediator;
        private readonly IDictionary<string, Stream> _Streams = new Dictionary<string, Stream>();

        public Store(IMediator mediator)
        {
            _Mediator = mediator;
        }

        public Stream Open(string streamName)
        {
            if (_Streams.ContainsKey(streamName))
            {
                return _Streams[streamName];
            }

            var stream = new Stream(streamName, _Mediator);
            _Streams[streamName] = stream;

            return stream;
        }
    }
}
