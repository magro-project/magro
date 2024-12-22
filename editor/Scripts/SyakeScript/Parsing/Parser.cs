using Magro.Scripts.MiddleLevel;
using System.Collections.Generic;
using System.IO;

namespace Magro.Scripts.SyakeScript.Parsing
{
    internal partial class Parser
    {
        public void Parse()
        {
            var reader = new StreamReader("script.ss");
            var scan = new Scanner(reader);
            ParseDeclaration(scan);
        }

        #region Common

        public Block ParseBlock(Scanner scan)
        {
            scan.Expect(TokenKind.OpenBrace);

            var statements = new List<IStatement>();

            // TODO

            // ParseStatement(scan);

            scan.Expect(TokenKind.CloseBrace);

            return new Block()
            {
                Statements = statements,
            };
        }

        public List<string> ParseParameters(Scanner scan)
        {
            scan.Expect(TokenKind.OpenParen);

            var parameters = new List<string>();

            // TODO

            scan.Expect(TokenKind.CloseParen);

            return parameters;
        }

        #endregion Common
    }
}
