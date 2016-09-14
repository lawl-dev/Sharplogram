using MTProto.Streaming;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Core
{
    public abstract class TLObject : ISerializable
    {
        public abstract uint GetConstructorId();

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream stream)
        {
            StreamUtil.WriteInt(GetConstructorId(), stream);
            throw new NotImplementedException();
        }
    }
}
