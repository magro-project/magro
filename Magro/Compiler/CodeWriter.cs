using System.Text;

namespace Magro.Compiler
{
    internal class CodeWriter
    {
        private StringBuilder Builder { get; set; }
        private int IndentLevel { get; set; }

        public CodeWriter()
        {
            Builder = new StringBuilder();
            IndentLevel = 0;
        }

        public void EnterIndent()
        {
            IndentLevel++;
        }

        public void LeaveIndent()
        {
            IndentLevel--;
        }

        public void WriteIndent()
        {
            for (int i = 0; i < IndentLevel; i++)
            {
                Builder.Append("\t");
            }
        }

        public void Write(string str)
        {
            Builder.Append(str);
        }

        public void WriteLine(string str)
        {
            Builder.AppendLine(str);
        }

        public void WriteLine()
        {
            Builder.AppendLine();
        }

        public string GetCode()
        {
            return Builder.ToString();
        }
    }
}
