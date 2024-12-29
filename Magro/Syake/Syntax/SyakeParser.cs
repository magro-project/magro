using System.Collections.Generic;
using System.IO;

namespace Magro.Syake.Syntax
{
    internal partial class SyakeParser
    {
        public SyModuleDeclaration Parse(string moduleName, StreamReader source)
        {
            var reader = new SyakeTokenReader(source);
            var statements = new List<ISyStatement>();

            while (!reader.Is(TokenKind.EOF))
            {
                statements.AddRange(ParseStatement(reader));
            }

            return new SyModuleDeclaration()
            {
                Name = moduleName,
                Statements = statements,
            };
        }
    }
}
