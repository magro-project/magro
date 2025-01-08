using System.IO;

namespace Magro.Compiler
{
    internal class SyakeCompiler
    {
        public void Compile(StreamReader input, StreamWriter output)
        {
            // parse syake
            var parser = new SyakeParser();
            var scanner = new SyakeScanner(input);
            var module = parser.Parse(scanner, "main");

            // convert to IR
            var converter = new IrConverter();
            var irModule = converter.ConvertModule(module);

            // generate go code
            var codegen = new GolangGenerator();
            var writer = new CodeWriter();
            codegen.Generate(writer, irModule);

            output.Write(writer.GetCode());
        }
    }
}
