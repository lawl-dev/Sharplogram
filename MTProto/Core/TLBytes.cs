using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Core
{
    public class TLBytes
    {
        public byte[] Data { get; }
        public int Offset { get; }
        public int Length { get; }

        public TLBytes(byte[] data)
        {
            this.Data = data;
            this.Offset = 0;
            this.Length = data.Length;
        }

        public TLBytes(byte[] data, int offset, int len)
        {
            this.Data = data;
            this.Offset = offset;
            this.Length = len;
        }

    public override bool Equals(object obj)
        {
            if (!(obj is TLBytes))
            return false;
            if (this == obj)
                return true;

            TLBytes o = (TLBytes)obj;
            return Offset == o.Offset
                    && Length == o.Length
                    && Enumerable.SequenceEqual(Data, o.Data);
        }
    }
}
