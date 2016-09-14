using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Secure
{
    class RandomUtils
    {
        private static RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

        public static byte[] RandomSessionId()
        {
            return RandomByteArray(8);
        }

        public static int RandomInt()
        {
            return BitConverter.ToInt32(RandomByteArray(4), 0);
        }

        public static long RandomLong()
        {
            return BitConverter.ToInt64(RandomByteArray(8), 0);
        }

        private static byte[] RandomByteArray(int length)
        {
            byte[] buffer = new byte[length];
            random.GetNonZeroBytes(buffer);
            return buffer;
        }
    }
}
