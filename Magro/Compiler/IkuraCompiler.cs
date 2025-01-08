using System.IO;

namespace Magro.Compiler
{
    internal class IkuraCompiler
    {
        public void Compile(IkModuleDeclaration module, StreamWriter output)
        {
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
