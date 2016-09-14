using MTProto.Streaming;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Core
{
    public abstract class TLBool : TLObject
    {

        public static readonly TLBool TRUE = new TLBoolTrue();
        public static readonly TLBool FALSE = new TLBoolFalse();

        public static readonly uint TRUE_CONSTRUCTOR_ID = TLBoolTrue.CONSTRUCTOR_ID;
        public static readonly uint FALSE_CONSTRUCTOR_ID = TLBoolFalse.CONSTRUCTOR_ID;

        public static TLBool Get(bool value)
        {
            return value ? TRUE : FALSE;
        }

        public static void Serialize(bool value, Stream stream)
        {
            Get(value).Serialize(stream);
        }

        public static bool Deserialize(Stream stream)
        {
            uint constructorId = StreamUtil.ReadInt(stream);
            if (constructorId == TLBoolTrue.CONSTRUCTOR_ID)
                return true;
            if (constructorId == TLBoolFalse.CONSTRUCTOR_ID)
                return false;

            throw new InvalidConstructorIdException("Wrong TLBool constructor id. Found " + Int32.toHexString(constructorId)
                                                            + ", expected: " + Int32.toHexString(TLBoolTrue.CONSTRUCTOR_ID)
                                                            + " or " + Int32.toHexString(TLBoolFalse.CONSTRUCTOR_ID));
        }

        public override bool Equals(object obj)
        {
            return this == obj; // Singleton, 1 instance
        }

        public class TLBoolTrue : TLBool
        {

            public const uint CONSTRUCTOR_ID = 0x997275b5;

            public TLBoolTrue()
            {

            }

            public override uint GetConstructorId()
            {
                return CONSTRUCTOR_ID;
            }


            public override string ToString()
            {
                return "boolTrue#997275b5";
            }
        }

        public class TLBoolFalse : TLBool
        {

            public readonly uint CONSTRUCTOR_ID = 0xbc799737;

            public TLBoolFalse()
            {

            }

            public override uint GetConstructorId()
            {
                return CONSTRUCTOR_ID;
            }

            public override string ToString()
            {
                return "boolFalse#bc799737";
            }
        }
    }
}
