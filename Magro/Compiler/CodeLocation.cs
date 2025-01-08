namespace Magro.Compiler
{
    internal class CodeLocation
    {
        public int Line { get; private set; }
        public int Column { get; private set; }

        public CodeLocation(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return $"{Line}:{Column}";
        }
    }
}
