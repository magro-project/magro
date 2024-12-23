using Magro.Common.MiddleLevel;
using System.Collections.Generic;
using System.IO;

namespace Magro.Syake.Parsing
{
    internal partial class Parser
    {
        public ModuleDeclaration Parse(string moduleName, StreamReader reader)
        {
            var scan = new Scanner(reader);
            var statements = new List<IStatement>();

            while (!scan.Is(TokenKind.EOF))
            {
                statements.Add(ParseStatement(scan));
            }

            return new ModuleDeclaration()
            {
                Name = moduleName,
                Statements = statements,
            };
        }
    }
}
