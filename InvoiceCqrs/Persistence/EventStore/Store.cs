using System;
using System.Collections.Generic;
using InvoiceCqrs.Util;
using MediatR;

namespace InvoiceCqrs.Persistence.EventStore
{
    public class Store
    {
        private readonly IMediator _Mediator;
        private readonly IGuidGenerator _GuidGenerator;
        private readonly IDictionary<Guid, Stream> _Streams = new Dictionary<Guid, Stream>();

        public Store(IMediator mediator, IGuidGenerator guidGenerator)
        {
            _Mediator = mediator;
            _GuidGenerator = guidGenerator;
        }

        public Stream Open(Guid streamId)
        {
            if (_Streams.ContainsKey(streamId))
            {
                return _Streams[streamId];
            }


            var stream = new Stream(streamId, _Mediator, _GuidGenerator);
            _Streams[streamId] = stream;

            return stream;
        }
    }
}
