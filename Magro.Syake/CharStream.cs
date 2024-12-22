using System.IO;

namespace Magro.Syake
{
    internal class CharStream
    {
        private StreamReader Reader;

        public int Line { get; private set; }
        public int Column { get; private set; }
        public char? CurrentChar { get; private set; }

        public bool EndOfStream
        {
            get => CurrentChar == null;
        }

        public CharStream(StreamReader reader)
        {
            Reader = reader;
            CurrentChar = null;
            Line = 1;
            Column = 1;
            CurrentChar = ReadOne();
        }

        public Location GetLocation()
        {
            return new Location(Line, Column);
        }

        public void Next()
        {
            if (EndOfStream)
                return;

            // consume LF
            if (CurrentChar == '\n')
            {
                Line++;
                Column = 1;
            }
            else
            {
                Column++;
            }

            CurrentChar = ReadOne();

            // consume CR
            while (CurrentChar == '\r')
            {
                CurrentChar = ReadOne();
            }
        }

        private char? ReadOne()
        {
            if (Reader.EndOfStream)
            {
                return null;
            }
            else
            {
                return (char)Reader.Read();
            }
        }
    }
}
