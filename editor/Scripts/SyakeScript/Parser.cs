using System.Collections.Generic;
using System.IO;

namespace Magro.Scripts.SyakeScript
{
    internal class Parser
    {
        public Parser()
        {
            var reader = new StreamReader("script.sk");
            var scan = new Scanner(reader);

            ParseStatement(scan);
        }

        public void ParseDeclaration(Scanner scan)
        {
            if (scan.Is("function"))
            {
                scan.Next();
                scan.Expect(TokenKind.Word);
                ParseParameters(scan);
                ParseBlock(scan);
            }

            if (scan.Is("var"))
            {
                scan.Next();
                scan.Expect(TokenKind.Word);

                if (scan.Is(TokenKind.Equal))
                {
                    scan.Next();
                    ParseExpression(scan);
                }

                scan.Expect(TokenKind.SemiCollon);
            }
        }

        public void ParseStatement(Scanner scan)
        {
            if (scan.Is("if"))
            {
                scan.Next();
                scan.Expect(TokenKind.OpenParen);
                ParseExpression(scan);
                scan.Expect(TokenKind.CloseParen);

                if (scan.Is(TokenKind.OpenBrace))
                {
                    ParseBlock(scan);
                }
                else
                {
                    ParseExpression(scan);
                }

                if (scan.Is("else"))
                {
                    scan.Next();

                    if (scan.Is(TokenKind.OpenBrace))
                    {
                        ParseBlock(scan);
                    }
                    else
                    {
                        ParseExpression(scan);
                    }
                }
            }

            // assign

            // increment

            // decrement

            // expression statement
        }

        public void ParseBlock(Scanner scan)
        {
            scan.Expect(TokenKind.OpenBrace);

            // TODO

            // ParseStatement(scan);
            
            scan.Expect(TokenKind.CloseBrace);
        }

        public void ParseParameters(Scanner scan)
        {
            scan.Expect(TokenKind.OpenParen);

            // TODO

            scan.Expect(TokenKind.CloseParen);
        }

        public void ParseExpression(Scanner scan)
        {

        }
    }
}
