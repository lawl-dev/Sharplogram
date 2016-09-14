using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Secure
{
    public static class CryptoUtils
    {
        private static SHA1 _sha1 = new SHA1CryptoServiceProvider();

        public static byte[] SHA1(byte[] src)
        {
            return _sha1.TransformFinalBlock(src, 0, src.Length);
        }

        public static byte[] Substring(byte[] src, int start, int len)
        {
            byte[] res = new byte[len];
            Array.Copy(src, start, res, 0, len);
            return res;
        }
    }
}
