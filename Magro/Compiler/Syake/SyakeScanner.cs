using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace Magro.Compiler
{
    internal class SyakeScanner
    {
        private CodeReader Reader;
        private List<SyakeToken> Tokens = new List<SyakeToken>();

        public SyakeScanner(StreamReader streamReader)
        {
            Reader = new CodeReader(streamReader);
            Tokens.Add(ReadOne());
        }

        public SyakeToken GetToken()
        {
            return Tokens[0];
        }

        public TokenKind GetTokenKind()
        {
            return Tokens[0].TokenKind;
        }

        public string GetTokenContent()
        {
            return Tokens[0].Content;
        }

        public bool Is(TokenKind kind)
        {
            return (Tokens[0].TokenKind == kind);
        }

        public bool Is(string word)
        {
            if (Tokens[0].TokenKind != TokenKind.Word) return false;
            return Tokens[0].Content == word;
        }

        public void Expect(TokenKind kind)
        {
            if (!Is(kind))
            {
                var token = Tokens[0];
                throw new ApplicationException($"Unexpected token {token} ({token.BeginLocation} - {token.EndLocation})");
            }
        }

        public void Expect(string word)
        {
            if (!Is(word))
            {
                var token = Tokens[0];
                throw new ApplicationException($"Unexpected token {token} ({token.BeginLocation} - {token.EndLocation})");
            }
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

        public SyakeToken Peek(int offset)
        {
            while (Tokens.Count <= offset)
            {
                Tokens.Add(ReadOne());
            }

            return Tokens[offset];
        }

        private SyakeToken ReadOne()
        {
            while (true)
            {
                if (Reader.GetChar() == null)
                {
                    var location = Reader.GetLocation();
                    return new SyakeToken(TokenKind.EOF, location, location);
                }

                // skip spacing
                if (IsSpacingChar(Reader.GetChar()))
                {
                    Reader.Next();
                    continue;
                }

                var begin = Reader.GetLocation();

                switch (Reader.GetChar())
                {
                    case '[':
                        Reader.Next();
                        return new SyakeToken(TokenKind.OpenBracket, begin, Reader.GetLocation());

                    case ']':
                        Reader.Next();
                        return new SyakeToken(TokenKind.CloseBracket, begin, Reader.GetLocation());

                    case '{':
                        Reader.Next();
                        return new SyakeToken(TokenKind.OpenBrace, begin, Reader.GetLocation());

                    case '}':
                        Reader.Next();
                        return new SyakeToken(TokenKind.CloseBrace, begin, Reader.GetLocation());

                    case '(':
                        Reader.Next();
                        return new SyakeToken(TokenKind.OpenParen, begin, Reader.GetLocation());

                    case ')':
                        Reader.Next();
                        return new SyakeToken(TokenKind.CloseParen, begin, Reader.GetLocation());

                    case ',':
                        Reader.Next();
                        return new SyakeToken(TokenKind.Comma, begin, Reader.GetLocation());

                    case '.':
                        Reader.Next();
                        return new SyakeToken(TokenKind.Dot, begin, Reader.GetLocation());

                    case ';':
                        Reader.Next();
                        return new SyakeToken(TokenKind.SemiCollon, begin, Reader.GetLocation());

                    case '=':
                        Reader.Next();
                        if (Reader.GetChar() == '=')
                        {
                            Reader.Next();
                            return new SyakeToken(TokenKind.Equal2, begin, Reader.GetLocation());
                        }
                        return new SyakeToken(TokenKind.Equal, begin, Reader.GetLocation());

                    case '>':
                        Reader.Next();
                        if (Reader.GetChar() == '=')
                        {
                            Reader.Next();
                            return new SyakeToken(TokenKind.GtEq, begin, Reader.GetLocation());
                        }
                        return new SyakeToken(TokenKind.Gt, begin, Reader.GetLocation());

                    case '<':
                        Reader.Next();
                        if (Reader.GetChar() == '=')
                        {
                            Reader.Next();
                            return new SyakeToken(TokenKind.LtEq, begin, Reader.GetLocation());
                        }
                        return new SyakeToken(TokenKind.Lt, begin, Reader.GetLocation());

                    case '!':
                        Reader.Next();
                        return new SyakeToken(TokenKind.Not, begin, Reader.GetLocation());

                    case '+':
                        Reader.Next();
                        if (Reader.GetChar() == '+')
                        {
                            Reader.Next();
                            return new SyakeToken(TokenKind.Plus2, begin, Reader.GetLocation());
                        }
                        return new SyakeToken(TokenKind.Plus, begin, Reader.GetLocation());

                    case '-':
                        Reader.Next();
                        if (Reader.GetChar() == '-')
                        {
                            Reader.Next();
                            return new SyakeToken(TokenKind.Minus2, begin, Reader.GetLocation());
                        }
                        return new SyakeToken(TokenKind.Minus, begin, Reader.GetLocation());

                    case '*':
                        Reader.Next();
                        return new SyakeToken(TokenKind.Astarisk, begin, Reader.GetLocation());

                    case '/':
                        Reader.Next();
                        return new SyakeToken(TokenKind.Slash, begin, Reader.GetLocation());

                    case '%':
                        Reader.Next();
                        return new SyakeToken(TokenKind.Percent, begin, Reader.GetLocation());

                    case '&':
                        Reader.Next();
                        if (Reader.GetChar() == '&')
                        {
                            Reader.Next();
                            return new SyakeToken(TokenKind.And2, begin, Reader.GetLocation());
                        }
                        throw new ApplicationException($"Invalid character ({begin})");

                    case '|':
                        Reader.Next();
                        if (Reader.GetChar() == '|')
                        {
                            Reader.Next();
                            return new SyakeToken(TokenKind.Or2, begin, Reader.GetLocation());
                        }
                        throw new ApplicationException($"Invalid character ({begin})");
                }

                SyakeToken token;

                if (TryReadString(out token))
                {
                    return token;
                }

                if (TryReadNumber(out token))
                {
                    return token;
                }

                if (TryReadWord(out token))
                {
                    return token;
                }

                throw new ApplicationException($"Invalid character ({begin})");
            }
        }

        private bool TryReadString(out SyakeToken token)
        {
            token = null;

            var buf = new StringBuilder();
            var begin = Reader.GetLocation();

            if (Reader.GetChar() != '"')
            {
                return false;
            }
            Reader.Next();

            while (Reader.GetChar() != null)
            {
                var ch = Reader.GetChar();
                if (ch == '"') break;
                buf.Append(ch);
                Reader.Next();
            }

            if (Reader.GetChar() != '"')
            {
                throw new ApplicationException($"'\"' expected. ({Reader.GetLocation()})");
            }
            Reader.Next();

            if (buf.Length > 0)
            {
                buf.ToString();
                token = new SyakeToken(TokenKind.String, begin, Reader.GetLocation())
                {
                    Content = buf.ToString()
                };
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TryReadNumber(out SyakeToken token)
        {
            token = null;

            var buf = new StringBuilder();
            var begin = Reader.GetLocation();

            while (Reader.GetChar() != null)
            {
                var first = (buf.Length == 0);
                var ch = Reader.GetChar();
                if (!IsNumberChar(ch, first)) break;
                buf.Append(ch);
                Reader.Next();
            }

            if (Reader.GetChar() == '.')
            {
                var buf2 = new StringBuilder();
                buf2.Append(Reader.GetChar());
                Reader.Next();

                var numberLocation2 = Reader.GetLocation();

                while (Reader.GetChar() != null)
                {
                    var first = (buf2.Length == 0);
                    var ch = Reader.GetChar();
                    if (!IsNumberChar(ch, first)) break;
                    buf2.Append(ch);
                    Reader.Next();
                }

                if (buf2.Length > 0)
                {
                    buf.Append(buf2.ToString());
                }
                else
                {
                    throw new ApplicationException($"Number expected. ({numberLocation2})");
                }
            }

            if (buf.Length > 0)
            {
                buf.ToString();
                token = new SyakeToken(TokenKind.Number, begin, Reader.GetLocation())
                {
                    Content = buf.ToString()
                };
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TryReadWord(out SyakeToken token)
        {
            token = null;

            var buf = new StringBuilder();
            var begin = Reader.GetLocation();

            while (Reader.GetChar() != null)
            {
                var ch = Reader.GetChar();
                if (!IsIdentifierChar(ch)) break;
                buf.Append(ch);
                Reader.Next();
            }

            if (buf.Length > 0)
            {
                token = new SyakeToken(TokenKind.Word, begin, Reader.GetLocation())
                {
                    Content = buf.ToString()
                };
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsNumberChar(char? ch, bool first)
        {
            if (ch == null) return false;

            var code = (int)ch;

            if (!first && code == 0x30) return true;
            if (code >= 0x31 && code <= 0x39) return true;

            return false;
        }

        private bool IsIdentifierChar(char? ch)
        {
            if (ch == null) return false;

            var code = (int)ch;

            if (code >= 0x30 && code <= 0x39) return true;
            if (code >= 0x41 && code <= 0x5A) return true;
            if (code == 0x5F) return true;
            if (code >= 0x61 && code <= 0x7A) return true;

            return false;
        }

        private bool IsSpacingChar(char? ch)
        {
            if (ch == null) return false;

            var code = (int)ch;

            // tab
            if (code == 0x09) return true;
            // LF
            if (code == 0x0A) return true;
            // CR
            if (code == 0x0D) return true;
            // space
            if (code == 0x20) return true;

            return false;
        }
    }
}
