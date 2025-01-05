using Magro.Ir;
using System.Collections.Generic;
using System.Text;

namespace Magro.Golang.GolangAst
{
    internal class GeneratorContext
    {
        private StringBuilder Builder { get; set; }
        private int IndentLevel { get; set; }

        public GeneratorContext()
        {
            Builder = new StringBuilder();
            IndentLevel = 0;
        }

        public void Write(string str)
        {
            Builder.Append(str);
        }

        public void WriteLine(string str)
        {
            Builder.AppendLine(str);
        }

        public void WriteWithIndent(string str)
        {
            for (int i = 0; i < IndentLevel; i++)
            {
                Builder.Append("  ");
            }

            Builder.Append(str);
        }

        public void WriteLineWithIndent(string str)
        {
            for (int i = 0; i < IndentLevel; i++)
            {
                Builder.Append("  ");
            }

            Builder.AppendLine(str);
        }

        public string GetCode()
        {
            return Builder.ToString();
        }
    }

    internal class GolangGenerator
    {
        public void Generate(GeneratorContext ctx, IrModuleDeclaration module)
        {
            
        }

        public void EmitStatement(GeneratorContext ctx, IrStatement statement)
        {

        }
    }
}
