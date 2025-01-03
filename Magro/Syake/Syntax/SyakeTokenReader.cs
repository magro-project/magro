﻿using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace Magro.Syake.Syntax
{
    internal class SyakeTokenReader
    {
        private CharStream Stream;
        private List<SyakeToken> Tokens = new List<SyakeToken>();

        public SyakeTokenReader(StreamReader reader)
        {
            Stream = new CharStream(reader);
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
                if (Stream.GetChar() == null)
                {
                    var location = Stream.GetLocation();
                    return new SyakeToken(TokenKind.EOF, location, location);
                }

                // skip spacing
                if (IsSpacingChar(Stream.GetChar()))
                {
                    Stream.Next();
                    continue;
                }

                var begin = Stream.GetLocation();

                switch (Stream.GetChar())
                {
                    case '[':
                        Stream.Next();
                        return new SyakeToken(TokenKind.OpenBracket, begin, Stream.GetLocation());

                    case ']':
                        Stream.Next();
                        return new SyakeToken(TokenKind.CloseBracket, begin, Stream.GetLocation());

                    case '{':
                        Stream.Next();
                        return new SyakeToken(TokenKind.OpenBrace, begin, Stream.GetLocation());

                    case '}':
                        Stream.Next();
                        return new SyakeToken(TokenKind.CloseBrace, begin, Stream.GetLocation());

                    case '(':
                        Stream.Next();
                        return new SyakeToken(TokenKind.OpenParen, begin, Stream.GetLocation());

                    case ')':
                        Stream.Next();
                        return new SyakeToken(TokenKind.CloseParen, begin, Stream.GetLocation());

                    case ',':
                        Stream.Next();
                        return new SyakeToken(TokenKind.Comma, begin, Stream.GetLocation());

                    case '.':
                        Stream.Next();
                        return new SyakeToken(TokenKind.Dot, begin, Stream.GetLocation());

                    case ';':
                        Stream.Next();
                        return new SyakeToken(TokenKind.SemiCollon, begin, Stream.GetLocation());

                    case '=':
                        Stream.Next();
                        if (Stream.GetChar() == '=')
                        {
                            Stream.Next();
                            return new SyakeToken(TokenKind.Equal2, begin, Stream.GetLocation());
                        }
                        return new SyakeToken(TokenKind.Equal, begin, Stream.GetLocation());

                    case '>':
                        Stream.Next();
                        if (Stream.GetChar() == '=')
                        {
                            Stream.Next();
                            return new SyakeToken(TokenKind.GtEq, begin, Stream.GetLocation());
                        }
                        return new SyakeToken(TokenKind.Gt, begin, Stream.GetLocation());

                    case '<':
                        Stream.Next();
                        if (Stream.GetChar() == '=')
                        {
                            Stream.Next();
                            return new SyakeToken(TokenKind.LtEq, begin, Stream.GetLocation());
                        }
                        return new SyakeToken(TokenKind.Lt, begin, Stream.GetLocation());

                    case '!':
                        Stream.Next();
                        return new SyakeToken(TokenKind.Not, begin, Stream.GetLocation());

                    case '+':
                        Stream.Next();
                        if (Stream.GetChar() == '+')
                        {
                            Stream.Next();
                            return new SyakeToken(TokenKind.Plus2, begin, Stream.GetLocation());
                        }
                        return new SyakeToken(TokenKind.Plus, begin, Stream.GetLocation());

                    case '-':
                        Stream.Next();
                        if (Stream.GetChar() == '-')
                        {
                            Stream.Next();
                            return new SyakeToken(TokenKind.Minus2, begin, Stream.GetLocation());
                        }
                        return new SyakeToken(TokenKind.Minus, begin, Stream.GetLocation());

                    case '*':
                        Stream.Next();
                        return new SyakeToken(TokenKind.Astarisk, begin, Stream.GetLocation());

                    case '/':
                        Stream.Next();
                        return new SyakeToken(TokenKind.Slash, begin, Stream.GetLocation());

                    case '%':
                        Stream.Next();
                        return new SyakeToken(TokenKind.Percent, begin, Stream.GetLocation());

                    case '&':
                        Stream.Next();
                        if (Stream.GetChar() == '&')
                        {
                            Stream.Next();
                            return new SyakeToken(TokenKind.And2, begin, Stream.GetLocation());
                        }
                        throw new ApplicationException($"Invalid character ({begin})");

                    case '|':
                        Stream.Next();
                        if (Stream.GetChar() == '|')
                        {
                            Stream.Next();
                            return new SyakeToken(TokenKind.Or2, begin, Stream.GetLocation());
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
            var begin = Stream.GetLocation();

            if (Stream.GetChar() != '"')
            {
                return false;
            }
            Stream.Next();

            while (Stream.GetChar() != null)
            {
                var ch = Stream.GetChar();
                if (ch == '"') break;
                buf.Append(ch);
                Stream.Next();
            }

            if (Stream.GetChar() != '"')
            {
                throw new ApplicationException($"'\"' expected. ({Stream.GetLocation()})");
            }
            Stream.Next();

            if (buf.Length > 0)
            {
                buf.ToString();
                token = new SyakeToken(TokenKind.String, begin, Stream.GetLocation())
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
            var begin = Stream.GetLocation();

            while (Stream.GetChar() != null)
            {
                var first = (buf.Length == 0);
                var ch = Stream.GetChar();
                if (!IsNumberChar(ch, first)) break;
                buf.Append(ch);
                Stream.Next();
            }

            if (Stream.GetChar() == '.')
            {
                var buf2 = new StringBuilder();
                buf2.Append(Stream.GetChar());
                Stream.Next();

                var numberLocation2 = Stream.GetLocation();

                while (Stream.GetChar() != null)
                {
                    var first = (buf2.Length == 0);
                    var ch = Stream.GetChar();
                    if (!IsNumberChar(ch, first)) break;
                    buf2.Append(ch);
                    Stream.Next();
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
                token = new SyakeToken(TokenKind.Number, begin, Stream.GetLocation())
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
            var begin = Stream.GetLocation();

            while (Stream.GetChar() != null)
            {
                var ch = Stream.GetChar();
                if (!IsIdentifierChar(ch)) break;
                buf.Append(ch);
                Stream.Next();
            }

            if (buf.Length > 0)
            {
                token = new SyakeToken(TokenKind.Word, begin, Stream.GetLocation())
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
