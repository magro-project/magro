using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Magro.MiddleLevel
{
    internal class SerializerContext
    {
        public BinaryWriter Writer { get; set; }

        public SerializerContext(BinaryWriter writer)
        {
            Writer = writer;
        }

        public void WriteByte(byte value)
        {
            Writer.Write(value);
        }

        public void WriteBytes(IEnumerable<byte> values)
        {
            Writer.Write(values.ToArray());
        }

        public void WriteInt16(short value)
        {
            Writer.Write(value);
        }

        public void WriteUInt16(ushort value)
        {
            Writer.Write(value);
        }

        public void WriteInt32(int value)
        {
            Writer.Write(value);
        }

        public void WriteUInt32(uint value)
        {
            Writer.Write(value);
        }

        public void WriteInt64(long value)
        {
            Writer.Write(value);
        }

        public void WriteUInt64(ulong value)
        {
            Writer.Write(value);
        }
    }
}
