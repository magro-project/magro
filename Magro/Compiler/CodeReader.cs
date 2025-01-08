using System.IO;

namespace Magro.Compiler
{
    internal class CodeReader
    {
        private StreamReader Reader;
        private int Line { get; set; }
        private int Column { get; set; }
        private char? CurrentChar { get; set; }

        public CodeReader(StreamReader reader)
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

        public CodeLocation GetLocation()
        {
            return new CodeLocation(Line, Column);
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
