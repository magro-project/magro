using Magro.Common.MiddleLevel;
using System;
using System.Collections.Generic;

namespace Magro.Syake.Parsing
{
    internal partial class Parser
    {
        public IDeclaration ParseDeclaration(Scanner scan)
        {
            if (scan.Is("function"))
            {
                scan.Next();
                scan.Expect(TokenKind.Word);
                var name = (string)scan.GetTokenContent();

                var parameters = ParseParameters(scan);
                var block = ParseBlock(scan);

                return new FunctionDeclaration()
                {
                    Name = name,
                    Parameters = parameters,
                    FunctionBlock = block,
                };
            }

            if (scan.Is("var"))
            {
                scan.Next();
                scan.Expect(TokenKind.Word);
                var name = (string)scan.GetTokenContent();

                IExpression initializer = null;
                if (scan.Is(TokenKind.Equal))
                {
                    scan.Next();
                    initializer = ParseExpression(scan);
                }

                scan.Expect(TokenKind.SemiCollon);

                return new VariableDeclaration()
                {
                    Name = name,
                    Initializer = initializer,
                };
            }

            throw new ApplicationException("Unexpected token");
        }
    }
}
