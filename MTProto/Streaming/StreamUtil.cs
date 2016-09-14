using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MTProto.Core;

namespace MTProto.Streaming
{
    public static class StreamUtil
    {
        public static void WriteByte(int v, Stream stream)
        {
            stream.WriteByte((byte)v);
        }

        public static void WriteByte(byte v, Stream stream)
        {
            stream.WriteByte(v);
        }

        public static void WriteByteArray(byte[] data, Stream stream)
        {
            stream.Write(data, 0, data.Length);
        }

        public static void WriteByteArray(byte[] data, int offset, int len, Stream stream)
        {
            stream.Write(data, offset, len);
        }

        public static void WriteInt(int v, Stream stream)
        {
            WriteByte((byte)(v & 0xFF), stream);
            WriteByte((byte)((v >> 8) & 0xFF), stream);
            WriteByte((byte)((v >> 16) & 0xFF), stream);
            WriteByte((byte)((v >> 24) & 0xFF), stream);

            WriteByte((byte)((v >> 32) & 0xFF), stream);
            WriteByte((byte)((v >> 40) & 0xFF), stream);
            WriteByte((byte)((v >> 48) & 0xFF), stream);
            WriteByte((byte)((v >> 56) & 0xFF), stream);
        }

        public static void WriteDouble(double v, Stream stream)
        {
            var bytes = BitConverter.GetBytes(v);
            stream.Write(bytes, 0, 1);
        }

        public static void WriteBoolean(bool v, Stream stream)
        {
            TLBool.Serialize(v, stream);
        }

        public static void WriteString(string v, Stream stream)
        {
            WriteTLBytes(Encoding.UTF8.GetBytes(v), stream);
        }

        public static void WriteTLBytes(byte[] v, Stream stream)
        {
            int startOffset = 1;
            if (v.Length >= 254)
            {
                startOffset = 4;
                WriteByte(254, stream);
                WriteByte(v.Length & 0xFF, stream);
                WriteByte((v.Length >> 8) & 0xFF, stream);
                WriteByte((v.Length >> 16) & 0xFF, stream);
            }
            else
            {
                WriteByte(v.Length, stream);
            }

            WriteByteArray(v, stream);

            int offset = (v.Length + startOffset) % 4;
            if (offset != 0)
            {
                int offsetCount = 4 - offset;
                WriteByteArray(new byte[offsetCount], stream);
            }
        }

        public static void writeTLBytes(TLBytes v, Stream stream)
        {
            int startOffset = 1;
            if (v.Length >= 254)
            {
                startOffset = 4;
                WriteByte(254, stream);
                WriteByte(v.Length & 0xFF, stream);
                WriteByte((v.Length >> 8) & 0xFF, stream);
                WriteByte((v.Length >> 16) & 0xFF, stream);
            }
            else
            {
                WriteByte(v.Length, stream);
            }

            WriteByteArray(v.Data, v.Offset, v.Length, stream);

            int offset = (v.Length + startOffset) % 4;
            if (offset != 0)
            {
                int offsetCount = 4 - offset;
                WriteByteArray(new byte[offsetCount], stream);
            }
        }

        public static void writeTLObject(TLObject v, Stream stream)
        {
            v.Serialize(stream);
        }

        public static void writeTLMethod<T>(TLMethod<T> v, Stream stream) where T : TLObject
        {
            writeTLObject(v, stream);
        }

        public static void writeTLVector(TLVector v, Stream stream)
        {
            writeTLObject(v, stream);
        }

        public static int readByte(Stream stream)
        {
            int a = stream.ReadByte();
            if (a < 0)
                throw new IOException();
            return a;
        }

        public static int ReadInt(Stream stream)
        {
            int a = stream.ReadByte();
            if (a < 0)
                throw new IOException();
            int b = stream.ReadByte();
            if (b < 0)
                throw new IOException();
            int c = stream.ReadByte();
            if (c < 0)
                throw new IOException();
            int d = stream.ReadByte();
            if (d < 0)
                throw new IOException();

            return a + (b << 8) + (c << 16) + (d << 24);
        }

        public static long readUInt(Stream stream)
        {
            long a = stream.ReadByte();
            if (a < 0)
            {
                throw new IOException();
            }
            long b = stream.ReadByte();
            if (b < 0)
            {
                throw new IOException();
            }
            long c = stream.ReadByte();
            if (c < 0)
            {
                throw new IOException();
            }
            long d = stream.ReadByte();
            if (d < 0)
            {
                throw new IOException();
            }

            return a + (b << 8) + (c << 16) + (d << 24);
        }

        public static long readLong(Stream stream)
        {
            long a = readUInt(stream);
            long b = readUInt(stream);

            return a + (b << 32);
        }
        public static double readDouble(Stream stream)
        {
            return Double.longBitsToDouble(readLong(stream));
        }

        public static String readTLString(Stream stream)
        {
            return new String(readTLBytes(stream), "UTF-8");
        }

        public static TLObject readTLObject(Stream stream, TLContext context)
        {
            return context.deserializeMessage(stream);
        }

        public static <T extends TLObject> T readTLObject(Stream stream, TLContext context, Class<T> clazz, int constructorId)
        {
            return context.deserializeMessage(stream, clazz, constructorId);
        }

        public static TLMethod readTLMethod(Stream stream, TLContext context)
        {
            return (TLMethod)context.deserializeMessage(stream);
        }

        public static byte[] readBytes(int count, Stream stream)
        {
            byte[]
        res = new byte[count];
            int offset = 0;
            while (offset < res.length)
            {
                int read = stream.read(res, offset, res.length - offset);
                if (read > 0)
                {
                    offset += read;
                }
                else if (read < 0)
                {
                    throw new IOException();
                }
                else
                {
                    Thread.yield();
                }
            }
            return res;
        }

        public static void skipBytes(int count, Stream stream)
        {
            readBytes(count, stream);
        }

        public static void readBytes(byte[] buffer, int offset, int count, Stream stream)
        {
            int woffset = 0;
            while (woffset < count)
            {
                int read = stream.read(buffer, woffset + offset, count - woffset);
                if (read > 0)
                {
                    woffset += read;
                }
                else if (read < 0)
                {
                    throw new IOException();
                }
                else
                {
                    Thread.yield();
                }
            }
        }

        public static byte[] readTLBytes(Stream stream)
        {
            int count = stream.read();
            int startOffset = 1;
            if (count >= 254)
            {
                count = stream.read() + (stream.read() << 8) + (stream.read() << 16);
                startOffset = 4;
            }

            byte[]
        raw = readBytes(count, stream);
            int offset = (count + startOffset) % 4;
            if (offset != 0)
            {
                int offsetCount = 4 - offset;
                skipBytes(offsetCount, stream);
            }

            return raw;
        }

        public static TLBytes readTLBytes(Stream stream, TLContext context)
        {
            int count = stream.read();
            int startOffset = 1;
            if (count >= 254)
            {
                count = stream.read() + (stream.read() << 8) + (stream.read() << 16);
                startOffset = 4;
            }

            TLBytes res = new TLBytes(new byte[count], 0, count);
            readBytes(res.getData(), res.getOffset(), res.getLength(), stream);

            int offset = (count + startOffset) % 4;
            if (offset != 0)
            {
                int offsetCount = 4 - offset;
                skipBytes(offsetCount, stream);
            }
            return res;
        }

        public static TLVector readTLVector(Stream stream, TLContext context)
        {
            return context.deserializeVector(stream);
        }

        public static TLIntVector readTLIntVector(Stream stream, TLContext context)
        {
            return context.deserializeIntVector(stream);
        }

        public static TLLongVector readTLLongVector(Stream stream, TLContext context)
        {
            return context.deserializeLongVector(stream);
        }

        public static TLStringVector readTLStringVector(Stream stream, TLContext context)
        {
            return context.deserializeStringVector(stream);
        }

        public static boolean readTLBool(Stream stream)
        {
            return TLBool.deserialize(stream);
        }

        public static byte[] intToBytes(int value)
        {
            return new byte[]{(byte) (value & 0xFF),
                          (byte) ((value >> 8) & 0xFF),
                          (byte) ((value >> 16) & 0xFF),
                          (byte) ((value >> 24) & 0xFF)};
        }

        public static byte[] longToBytes(long value)
        {
            return new byte[]{(byte) (value & 0xFF),
                          (byte) ((value >> 8) & 0xFF),
                          (byte) ((value >> 16) & 0xFF),
                          (byte) ((value >> 24) & 0xFF),
                          (byte) ((value >> 32) & 0xFF),
                          (byte) ((value >> 40) & 0xFF),
                          (byte) ((value >> 48) & 0xFF),
                          (byte) ((value >> 56) & 0xFF)};
        }

        public static int readInt(byte[] src)
        {
            return readInt(src, 0);
        }

        public static int readInt(byte[] src, int offset)
        {
            int a = src[offset] & 0xFF;
            int b = src[offset + 1] & 0xFF;
            int c = src[offset + 2] & 0xFF;
            int d = src[offset + 3] & 0xFF;

            return a + (b << 8) + (c << 16) + (d << 24);
        }

        public static long readUInt(byte[] src)
        {
            return readUInt(src, 0);
        }

        public static long readUInt(byte[] src, int offset)
        {
            long a = src[offset] & 0xFF;
            long b = src[offset + 1] & 0xFF;
            long c = src[offset + 2] & 0xFF;
            long d = src[offset + 3] & 0xFF;

            return a + (b << 8) + (c << 16) + (d << 24);
        }

        public static long readLong(byte[] src, int offset)
        {
            long a = readUInt(src, offset);
            long b = readUInt(src, offset + 4);

            return (a & 0xFFFFFFFF) + ((b & 0xFFFFFFFF) << 32);
        }

        private static final char[] hexArray = "0123456789ABCDEF".toCharArray();

        public static String toHexString(byte[] bytes)
        {
            char[] hexChars = new char[bytes.length * 2];
            for (int j = 0; j < bytes.length; j++)
            {
                int v = bytes[j] & 0xFF;
                hexChars[j * 2] = hexArray[v >>> 4];
                hexChars[j * 2 + 1] = hexArray[v & 0x0F];
            }
            return new String(hexChars);
        }
    }
}
