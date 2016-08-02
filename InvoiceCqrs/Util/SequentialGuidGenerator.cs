using System;
using System.Runtime.InteropServices;

namespace InvoiceCqrs.Util
{
    public class SequentialGuidGenerator : IGuidGenerator
    {
        public Guid Generate()
        {
            Guid guid;
            const int rpcOkResult = 0;

            var rpcResult = UuidCreateSequential(out guid);
            if (rpcResult != rpcOkResult)
            {
                throw new Exception("P/Invoke of UuidCreateSequential failed");
            }

            return guid;
        }

        [DllImport("rpcrt4.dll", SetLastError=true)]
        private static extern int UuidCreateSequential(out Guid guid);
    }
}
