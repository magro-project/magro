using System.Collections.Generic;

namespace Magro.Compiler
{
    internal partial class SyakeParser
    {
        public SyModuleDeclaration Parse(SyakeScanner scanner, string moduleName)
        {
            var statements = new List<SyStatement>();

            while (!scanner.Is(TokenKind.EOF))
            {
                statements.AddRange(ParseStatement(scanner));
            }

            return new SyModuleDeclaration()
            {
                Name = moduleName,
                Statements = statements,
            };
        }
    }
}
