using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MTProto.Core
{
    public abstract class TLMethod<T> : TLObject where T : TLObject
    {
        [ScriptIgnore]
        public T Response;

        public T deserializeResponse(byte[] data, TLContext context)
        {
            return Response = deserializeResponse(new MemoryStream(data), context);
        }

        public abstract T deserializeResponse(MemoryStream stream, TLContext context);
    }
}
