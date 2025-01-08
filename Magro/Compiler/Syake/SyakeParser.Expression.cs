using System;
using System.Collections.Generic;

namespace Magro.Compiler
{
    internal partial class SyakeParser
    {
        public SyExpression ParseExpression(SyakeScanner scanner)
        {
            return ParsePratt(scanner, 0);
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

        private SyExpression ParsePratt(SyakeScanner scanner, int minimumBindPower)
        {
            // pratt parsing
            // https://matklad.github.io/2020/04/13/simple-but-powerful-pratt-parsing.html

            SyExpression left;

            // find prefix operator
            var prefix = (PrefixOperator)Operators.Find(x =>
                x.OperatorKind == OperatorKind.PrefixOperator && ((PrefixOperator)x).TokenKind == scanner.GetTokenKind());
            if (prefix != null)
            {
                left = ParsePrefix(scanner);
            }
            else
            {
                left = ParseAtom(scanner);
            }

            while (true)
            {
                // find postfix operator
                var postfix = (PostfixOperator)Operators.Find(x =>
                    x.OperatorKind == OperatorKind.PostfixOperator && ((PostfixOperator)x).TokenKind == scanner.GetTokenKind());
                if (postfix != null)
                {
                    if (postfix.BindPower < minimumBindPower)
                    {
                        break;
                    }

                    left = ParsePostfix(scanner, left);
                    continue;
                }

                // find infix operator
                var infix = (InfixOperator)Operators.Find(x =>
                    x.OperatorKind == OperatorKind.InfixOperator && ((InfixOperator)x).TokenKind == scanner.GetTokenKind());
                if (infix != null)
                {
                    if (infix.LeftBindPower < minimumBindPower)
                    {
                        break;
                    }

                    left = ParseInfix(scanner, left, infix.RightBindPower);
                    continue;
                }

                break;
            }

            return left;
        }

        private SyExpression ParseAtom(SyakeScanner scanner)
        {
            if (scanner.Is(TokenKind.Number))
            {
                var value = scanner.GetTokenContent();
                scanner.Next();

                return new SyValueExpression()
                {
                    ValueKind = SyValueKind.Number,
                    Value = double.Parse(value),
                };
            }

            if (scanner.Is(TokenKind.String))
            {
                var value = scanner.GetTokenContent();
                scanner.Next();

                return new SyValueExpression()
                {
                    ValueKind = SyValueKind.String,
                    Value = value,
                };
            }

            if (scanner.Is("true"))
            {
                scanner.Next();

                return new SyValueExpression()
                {
                    ValueKind = SyValueKind.Boolean,
                    Value = true,
                };
            }

            if (scanner.Is("false"))
            {
                scanner.Next();

                return new SyValueExpression()
                {
                    ValueKind = SyValueKind.Boolean,
                    Value = false,
                };
            }

            if (scanner.Is("null"))
            {
                scanner.Next();

                return new SyValueExpression()
                {
                    ValueKind = SyValueKind.Null,
                    Value = null,
                };
            }

            if (scanner.Is(TokenKind.Word))
            {
                var name = scanner.GetTokenContent();
                scanner.Next();

                return new SyReferenceExpression()
                {
                    Name = name,
                };
            }

            // grouping operator
            if (scanner.Is(TokenKind.OpenParen))
            {
                scanner.Next();
                var expression = ParseExpression(scanner);
                scanner.Expect(TokenKind.CloseParen);
                scanner.Next();

                return new SyGroupingExpression()
                {
                    Expression = expression,
                };
            }

            throw new ApplicationException("Unexpected token " + scanner.GetToken());
        }

        private SyExpression ParsePrefix(SyakeScanner scanner)
        {
            var opTokenKind = scanner.GetTokenKind();
            scanner.Next();
            var target = ParseExpression(scanner);

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

            throw new ApplicationException("Unexpected token " + scanner.GetToken());
        }

        private SyExpression ParseInfix(SyakeScanner scanner, SyExpression left, int minimumBindPower)
        {
            var opTokenKind = scanner.GetTokenKind();
            scanner.Next();
            var right = ParsePratt(scanner, minimumBindPower);

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

            throw new ApplicationException("Unexpected token " + scanner.GetToken());
        }

        private SyExpression ParsePostfix(SyakeScanner scanner, SyExpression expr)
        {
            if (scanner.Is(TokenKind.OpenParen))
            {
                scanner.Next();

                var arguments = new List<SyExpression>();
                while (!scanner.Is(TokenKind.CloseParen))
                {
                    arguments.Add(ParseExpression(scanner));

                    if (scanner.Is(TokenKind.Comma))
                    {
                        scanner.Next();
                    }
                    else
                    {
                        break;
                    }
                }

                scanner.Expect(TokenKind.CloseParen);
                scanner.Next();

                return new SyCallFuncExpression()
                {
                    Target = expr,
                    Arguments = arguments,
                };
            }

            if (scanner.Is(TokenKind.OpenBracket))
            {
                scanner.Next();

                var indexes = new List<SyExpression>();
                while (!scanner.Is(TokenKind.CloseBracket))
                {
                    indexes.Add(ParseExpression(scanner));

                    if (scanner.Is(TokenKind.Comma))
                    {
                        scanner.Next();
                    }
                    else
                    {
                        break;
                    }
                }

                scanner.Expect(TokenKind.CloseBracket);
                scanner.Next();

                return new SyIndexAccessExpression()
                {
                    Target = expr,
                    Indexes = indexes,
                };
            }

            if (scanner.Is(TokenKind.Dot))
            {
                scanner.Next();
                scanner.Expect(TokenKind.Word);
                var memberName = scanner.GetTokenContent();
                scanner.Next();

                return new SyMemberAccessExpression()
                {
                    Target = expr,
                    MemberName = memberName,
                };
            }

            throw new ApplicationException("Unexpected token " + scanner.GetToken());
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
