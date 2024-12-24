using System.IO;

namespace Magro.Syake.Syntax
{
    internal class CharStream
    {
        private StreamReader Reader;
        private int Line { get; set; }
        private int Column { get; set; }
        private char? CurrentChar { get; set; }

        public CharStream(StreamReader reader)
        {
            Reader = reader;
            Line = 1;
            Column = 1;
            CurrentChar = ReadOne();
        }

        public char? GetChar()
        {
            return CurrentChar;
        }

        public Location GetLocation()
        {
            return new Location(Line, Column);
        }

        public void Next()
        {
            if (CurrentChar == null)
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
