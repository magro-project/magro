using editor.Scripts.SyakeScript;
using System.Collections.Generic;
using System.IO;

namespace Magro.Scripts.SyakeScript
{
    internal class Scanner
    {
        private CharStream Stream;
        private List<Token> Tokens = new List<Token>();

        public Token CurrentToken
        {
            get => Tokens[0];
        }

        public Scanner(StreamReader reader)
        {
            Stream = new CharStream(reader);
            Tokens.Add(ReadOne());
        }

        public void Next()
        {
            if (Tokens[0].TokenKind == TokenKind.EOF)
            {
                return;
            }

            Tokens.RemoveAt(0);

            if (Tokens.Count == 0)
            {
                Tokens.Add(ReadOne());
            }
        }

        public Token Peek(int offset)
        {
            while (Tokens.Count <= offset)
            {
                Tokens.Add(ReadOne());
            }

            return Tokens[offset];
        }

        private Token ReadOne()
        {
            while (true)
            {
                var begin = Stream.GetLocation();

                if (Stream.EndOfStream)
                {
                    return new Token(TokenKind.EOF, begin, Stream.GetLocation());
                }

                // TODO: spacing

                begin = Stream.GetLocation();

                switch (Stream.CurrentChar)
                {
                    case '[':
                        Stream.Next();
                        return new Token(TokenKind.OpenBracket, begin, Stream.GetLocation());

                    case ']':
                        Stream.Next();
                        return new Token(TokenKind.CloseBracket, begin, Stream.GetLocation());

                    case '{':
                        Stream.Next();
                        return new Token(TokenKind.OpenBrace, begin, Stream.GetLocation());

                    case '}':
                        Stream.Next();
                        return new Token(TokenKind.CloseBrace, begin, Stream.GetLocation());

                    case '(':
                        Stream.Next();
                        return new Token(TokenKind.OpenParen, begin, Stream.GetLocation());

                    case ')':
                        Stream.Next();
                        return new Token(TokenKind.CloseParen, begin, Stream.GetLocation());

                    case ',':
                        Stream.Next();
                        return new Token(TokenKind.Comma, begin, Stream.GetLocation());

                    case '.':
                        Stream.Next();
                        return new Token(TokenKind.Dot, begin, Stream.GetLocation());

                    case '=':
                        Stream.Next();
                        return new Token(TokenKind.Equal, begin, Stream.GetLocation());

                    case '!':
                        Stream.Next();
                        return new Token(TokenKind.Exclamation, begin, Stream.GetLocation());

                    case '+':
                        Stream.Next();
                        return new Token(TokenKind.Plus, begin, Stream.GetLocation());

                    case '-':
                        Stream.Next();
                        return new Token(TokenKind.Minus, begin, Stream.GetLocation());

                    case '*':
                        Stream.Next();
                        return new Token(TokenKind.Astarisk, begin, Stream.GetLocation());

                    case '/':
                        Stream.Next();
                        return new Token(TokenKind.Slash, begin, Stream.GetLocation());

                    case '%':
                        Stream.Next();
                        return new Token(TokenKind.Percent, begin, Stream.GetLocation());
                }

                // TODO: number

                // TODO: word
                
            }
        }
    }
}
