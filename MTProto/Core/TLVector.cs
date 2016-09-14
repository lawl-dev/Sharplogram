using MTProto.Core;
using MTProto.Streaming;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Core
{
    public class TLVector<T> : TLObject, IList<T>
    {

        public readonly TLVector<T> EMPTY_ARRAY = new TLVector<T>();

        public static readonly uint CONSTRUCTOR_ID = 0x1cb5c415;

        protected readonly Type itemClazz;
        protected readonly List<T> items = new List<T>();

        public TLVector()
        {
            itemClazz = typeof(TLObject);
        }

        public TLVector(T destClass)
        {
            if (destClass != null)
            {
                if (!(destClass is int) && !(destClass is int) && !(destClass is string)
                        && !typeof(TLObject).IsAssignableFrom(destClass.GetType()))
                {
                    throw new SystemException("Unsupported vector type: " + destClass.ToString());
                }
                itemClazz = destClass.GetType();
            }
            else
                itemClazz = typeof(TLObject);
        }

        
    public override string ToString()
        {
            return "vector#1cb5c415";
        }

        
    public override uint GetConstructorId()
        {
            return CONSTRUCTOR_ID;
        }

        
    public override void SerializeBody(Stream stream)
        {
            StreamUtil.WriteInt(items.Count, stream);
            for (T i : items)
                SerializeItem(i, stream);
        }

        protected void SerializeItem(T item, Stream stream)
        {
            writeTLObject((TLObject)item, stream);
        }

    @SuppressWarnings("unchecked")
    @Override
    public readonly void deserializeBody(InputStream stream, TLContext context)
        {
            int count = readInt(stream);
            for (int i = 0; i < count; i++)
                items.add(deserializeItem(stream, context));
        }

    @SuppressWarnings("unchecked")
    protected T deserializeItem(InputStream stream, TLContext context)
        {
            return (T)context.deserializeMessage(stream);
        }

        public override int computeSerializedSize()
        {
            int size = SIZE_CONSTRUCTOR_ID + SIZE_INT32; // Size
            for (T item : items)
                size += ((TLObject)item).computeSerializedSize();
            return size;
        }
    }

}
