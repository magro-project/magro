using System;
using System.Collections.Generic;

namespace Magro.Syake
{
    internal partial class SyakeParser
    {
        public SyExpression ParseExpression(SyakeTokenReader reader)
        {
            return ParsePratt(reader, 0);
        }

        public List<IOperator> Operators = new List<IOperator>()
        {
            new PostfixOperator(TokenKind.Dot,          34),
            new PostfixOperator(TokenKind.OpenBracket,  34),
            new PostfixOperator(TokenKind.OpenParen,    34),
            new PrefixOperator(TokenKind.Plus,          28),
            new PrefixOperator(TokenKind.Minus,         28),
            new PrefixOperator(TokenKind.Not,           28),
            new InfixOperator(TokenKind.Astarisk,       24, 25),
            new InfixOperator(TokenKind.Slash,          24, 25),
            new InfixOperator(TokenKind.Percent,        24, 25),
            new InfixOperator(TokenKind.Plus,           22, 23),
            new InfixOperator(TokenKind.Minus,          22, 23),
            new InfixOperator(TokenKind.Lt,             18, 19),
            new InfixOperator(TokenKind.LtEq,           18, 19),
            new InfixOperator(TokenKind.Gt,             18, 19),
            new InfixOperator(TokenKind.GtEq,           18, 19),
            new InfixOperator(TokenKind.Equal2,         16, 17),
            new InfixOperator(TokenKind.NotEqual,       16, 17),
            new InfixOperator(TokenKind.And2,            8,  9),
            new InfixOperator(TokenKind.Or2,             6,  7),
        };

        private SyExpression ParsePratt(SyakeTokenReader reader, int minimumBindPower)
        {
            // pratt parsing
            // https://matklad.github.io/2020/04/13/simple-but-powerful-pratt-parsing.html

            SyExpression left;

            // find prefix operator
            var prefix = (PrefixOperator)Operators.Find(x =>
                x.OperatorKind == OperatorKind.PrefixOperator && ((PrefixOperator)x).TokenKind == reader.GetTokenKind());
            if (prefix != null)
            {
                left = ParsePrefix(reader);
            }
            else
            {
                left = ParseAtom(reader);
            }

            while (true)
            {
                // find postfix operator
                var postfix = (PostfixOperator)Operators.Find(x =>
                    x.OperatorKind == OperatorKind.PostfixOperator && ((PostfixOperator)x).TokenKind == reader.GetTokenKind());
                if (postfix != null)
                {
                    if (postfix.BindPower < minimumBindPower)
                    {
                        break;
                    }

                    left = ParsePostfix(reader, left);
                    continue;
                }

                // find infix operator
                var infix = (InfixOperator)Operators.Find(x =>
                    x.OperatorKind == OperatorKind.InfixOperator && ((InfixOperator)x).TokenKind == reader.GetTokenKind());
                if (infix != null)
                {
                    if (infix.LeftBindPower < minimumBindPower)
                    {
                        break;
                    }

                    left = ParseInfix(reader, left, infix.RightBindPower);
                    continue;
                }

                break;
            }

            return left;
        }

        private SyExpression ParseAtom(SyakeTokenReader reader)
        {
            if (reader.Is(TokenKind.Number))
            {
                var value = reader.GetTokenContent();
                reader.Next();

                return new SyValueExpression()
                {
                    ValueKind = SyValueKind.Number,
                    Value = double.Parse(value),
                };
            }

            if (reader.Is(TokenKind.String))
            {
                var value = reader.GetTokenContent();
                reader.Next();

                return new SyValueExpression()
                {
                    ValueKind = SyValueKind.String,
                    Value = value,
                };
            }

            if (reader.Is("true"))
            {
                reader.Next();

                return new SyValueExpression()
                {
                    ValueKind = SyValueKind.Boolean,
                    Value = true,
                };
            }

            if (reader.Is("false"))
            {
                reader.Next();

                return new SyValueExpression()
                {
                    ValueKind = SyValueKind.Boolean,
                    Value = false,
                };
            }

            if (reader.Is("null"))
            {
                reader.Next();

                return new SyValueExpression()
                {
                    ValueKind = SyValueKind.Null,
                    Value = null,
                };
            }

            if (reader.Is(TokenKind.Word))
            {
                var name = reader.GetTokenContent();
                reader.Next();

                return new SyReferenceExpression()
                {
                    Name = name,
                };
            }

            // grouping operator
            if (reader.Is(TokenKind.OpenParen))
            {
                reader.Next();
                var expression = ParseExpression(reader);
                reader.Expect(TokenKind.CloseParen);
                reader.Next();

                return new SyGroupingExpression()
                {
                    Expression = expression,
                };
            }

            throw new ApplicationException("Unexpected token " + reader.GetToken());
        }

        private SyExpression ParsePrefix(SyakeTokenReader reader)
        {
            var opTokenKind = reader.GetTokenKind();
            reader.Next();
            var target = ParseExpression(reader);

            if (opTokenKind == TokenKind.Not)
            {
                return new SyNotOperator()
                {
                    Target = target,
                };
            }

            if (opTokenKind == TokenKind.Plus)
            {
                return new SySignExpression()
                {
                    SignKind = SySignKind.Positive,
                    Target = target,
                };
            }

            if (opTokenKind == TokenKind.Minus)
            {
                return new SySignExpression()
                {
                    SignKind = SySignKind.Negative,
                    Target = target,
                };
            }

            throw new ApplicationException("Unexpected token " + reader.GetToken());
        }

        private SyExpression ParseInfix(SyakeTokenReader reader, SyExpression left, int minimumBindPower)
        {
            var opTokenKind = reader.GetTokenKind();
            reader.Next();
            var right = ParsePratt(reader, minimumBindPower);

            if (opTokenKind == TokenKind.Plus)
            {
                return new SyMathOperator()
                {
                    MathOperatorKind = SyMathOperatorKind.Add,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.Minus)
            {
                return new SyMathOperator()
                {
                    MathOperatorKind = SyMathOperatorKind.Sub,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.Astarisk)
            {
                return new SyMathOperator()
                {
                    MathOperatorKind = SyMathOperatorKind.Mul,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.Slash)
            {
                return new SyMathOperator()
                {
                    MathOperatorKind = SyMathOperatorKind.Div,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.Percent)
            {
                return new SyMathOperator()
                {
                    MathOperatorKind = SyMathOperatorKind.Rem,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.Lt)
            {
                return new SyRelationalOperator()
                {
                    RelationalOperatorKind = SyRelationalOperatorKind.Lt,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.LtEq)
            {
                return new SyRelationalOperator()
                {
                    RelationalOperatorKind = SyRelationalOperatorKind.LtEq,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.Gt)
            {
                return new SyRelationalOperator()
                {
                    RelationalOperatorKind = SyRelationalOperatorKind.Gt,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.GtEq)
            {
                return new SyRelationalOperator()
                {
                    RelationalOperatorKind = SyRelationalOperatorKind.GtEq,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.Equal2)
            {
                return new SyRelationalOperator()
                {
                    RelationalOperatorKind = SyRelationalOperatorKind.Equal,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.NotEqual)
            {
                return new SyRelationalOperator()
                {
                    RelationalOperatorKind = SyRelationalOperatorKind.NotEqual,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.And2)
            {
                return new SyLogicOperator()
                {
                    LogicOperatorKind = SyLogicOperatorKind.And,
                    Left = left,
                    Right = right,
                };
            }

            if (opTokenKind == TokenKind.Or2)
            {
                return new SyLogicOperator()
                {
                    LogicOperatorKind = SyLogicOperatorKind.Or,
                    Left = left,
                    Right = right,
                };
            }

            throw new ApplicationException("Unexpected token " + reader.GetToken());
        }

        private SyExpression ParsePostfix(SyakeTokenReader reader, SyExpression expr)
        {
            if (reader.Is(TokenKind.OpenParen))
            {
                reader.Next();

                var arguments = new List<SyExpression>();
                while (!reader.Is(TokenKind.CloseParen))
                {
                    arguments.Add(ParseExpression(reader));

                    if (reader.Is(TokenKind.Comma))
                    {
                        reader.Next();
                    }
                    else
                    {
                        break;
                    }
                }

                reader.Expect(TokenKind.CloseParen);
                reader.Next();

                return new SyCallFuncExpression()
                {
                    Target = expr,
                    Arguments = arguments,
                };
            }

            if (reader.Is(TokenKind.OpenBracket))
            {
                reader.Next();

                var indexes = new List<SyExpression>();
                while (!reader.Is(TokenKind.CloseBracket))
                {
                    indexes.Add(ParseExpression(reader));

                    if (reader.Is(TokenKind.Comma))
                    {
                        reader.Next();
                    }
                    else
                    {
                        break;
                    }
                }

                reader.Expect(TokenKind.CloseBracket);
                reader.Next();

                return new SyIndexAccessExpression()
                {
                    Target = expr,
                    Indexes = indexes,
                };
            }

            if (reader.Is(TokenKind.Dot))
            {
                reader.Next();
                reader.Expect(TokenKind.Word);
                var memberName = reader.GetTokenContent();
                reader.Next();

                return new SyMemberAccessExpression()
                {
                    Target = expr,
                    MemberName = memberName,
                };
            }

            throw new ApplicationException("Unexpected token " + reader.GetToken());
        }
    }

    internal interface IOperator
    {
        OperatorKind OperatorKind { get; }
    }

    internal enum OperatorKind
    {
        PrefixOperator,
        InfixOperator,
        PostfixOperator,
    }

    internal class PrefixOperator : IOperator
    {
        public OperatorKind OperatorKind { get; } = OperatorKind.PrefixOperator;

        public TokenKind TokenKind { get; set; }
        public int BindPower { get; set; }

        public PrefixOperator(TokenKind tokenKind, int bindPower)
        {
            TokenKind = tokenKind;
            BindPower = bindPower;
        }
    }

    internal class InfixOperator : IOperator
    {
        public OperatorKind OperatorKind { get; } = OperatorKind.InfixOperator;

        public TokenKind TokenKind { get; set; }
        public int LeftBindPower { get; set; }
        public int RightBindPower { get; set; }

        public InfixOperator(TokenKind tokenKind, int leftBindPower, int rightBindPower)
        {
            TokenKind = tokenKind;
            LeftBindPower = leftBindPower;
            RightBindPower = rightBindPower;
        }
    }

    internal class PostfixOperator : IOperator
    {
        public OperatorKind OperatorKind { get; } = OperatorKind.PostfixOperator;

        public TokenKind TokenKind { get; set; }
        public int BindPower { get; set; }

        public PostfixOperator(TokenKind tokenKind, int bindPower)
        {
            TokenKind = tokenKind;
            BindPower = bindPower;
        }
    }
}
