using MTProto.Secure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Auth
{
    class AuthKey
    {
        public long KeyId { get; }

        public AuthKey(byte[] key)
        {
            if (key.Length != 256)
                throw new ArgumentOutOfRangeException("Key", "AuthKey mmust be 256 Bytes, found ${value.size} bytes.");

            KeyId = BitConverter.ToInt64(CryptoUtils.Substring(CryptoUtils.SHA1(key), 12, 8), 0);
        }
    }
}
