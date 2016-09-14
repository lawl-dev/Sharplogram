using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Core
{
    public abstract class TLContext
    {
        private Dictionary<int, Type> _registeredClasses;

        public TLContext(int size)
        {
            _registeredClasses = new Dictionary<int, Type>(size);
        }

        public bool IsSupportedObject(TLObject tlObject)
        {
            return IsSupportedObject(tlObject.GetConstructorId());
        }

        public bool IsSupportedObject(int constructorId)
        {
            return _registeredClasses.ContainsKey(constructorId);
        }

        public void registerClass(int constructorId, Type clazz)
        {
            _registeredClasses.Add(constructorId, clazz);
        }

        public TLObject DeserializeMessage(byte[] data)
        {
            return DeserializeMessage(data, null, -1);
        }

        public TLObject DeserializeMessage(byte[] data, Type clazz, int constructorId)
        {
            return DeserializeMessage(new MemoryStream(data), clazz, constructorId);
        }

        public TLObject DeserializeMessage(MemoryStream stream)
        {
            return DeserializeMessage(stream, null, -1);
        }

        public TLObject DeserializeMessage(MEmoryStream stream, Type clazz, int constructorId)
        {
        int realConstructorId = StreamUtils.readInt(stream);
        if (constructorId != -1 && realConstructorId != constructorId) {
            throw new InvalidConstructorIdException(realConstructorId, constructorId);
    } else if (constructorId == -1) {
            constructorId = realConstructorId;
            clazz = null;
        }

        if (constructorId == TLGzipObject.CONSTRUCTOR_ID)
            return (T) deserializeMessage(unzipStream(stream));
        if (constructorId == TLBool.TRUE_CONSTRUCTOR_ID)
            return (T) TLBool.TRUE;
        if (constructorId == TLBool.FALSE_CONSTRUCTOR_ID)
            return (T) TLBool.FALSE;
        if (constructorId == TLVector.CONSTRUCTOR_ID) {
            /* Vector should be deserialized via the appropriate method, a vector was not expected,
             we must assume it's not any of vector<int>, vector<long>, vector<string> */
            return (T) deserializeVectorBody(stream, new TLVector<>());
        }

        try {
            if (clazz == null) {
                clazz = registeredClasses.get(constructorId);
                if (clazz == null)
                    throw new UnsupportedConstructorException(constructorId);
            }

            T message = clazz.getConstructor().newInstance();
message.deserializeBody(stream, this);
            return message;
} catch (ReflectiveOperationException e) {
            // !! Should never happen
            throw new RuntimeException("Unable to deserialize data. This error should not happen", e);
        }
    }
    }
}
