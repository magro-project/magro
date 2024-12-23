using Magro.Common.MiddleLevel;
using System;
using System.Collections.Generic;

namespace Magro.Syake.Parsing
{
    internal partial class Parser
    {
        public IExpression ParseExpression(Scanner scan)
        {
            return ParsePratt(scan, 0);
        }

        public List<IOperator> Operators = new List<IOperator>()
        {
            new PostfixOperator(TokenKind.OpenParen,    45),
            new PostfixOperator(TokenKind.OpenBracket,  45),

//          new InfixOperator(TokenKind.Dot,            40, 41),

//          new PrefixOperator(TokenKind.Plus,          35),
//          new PrefixOperator(TokenKind.Minus,         35),
            new PrefixOperator(TokenKind.Not,           35),

            new InfixOperator(TokenKind.Astarisk,       30, 31),
            new InfixOperator(TokenKind.Slash,          30, 31),
            new InfixOperator(TokenKind.Percent,        30, 31),

            new InfixOperator(TokenKind.Plus,           25, 26),
            new InfixOperator(TokenKind.Minus,          25, 26),

//          new InfixOperator(TokenKind.Lt,             20, 21),
//          new InfixOperator(TokenKind.LtEq,           20, 21),
//          new InfixOperator(TokenKind.Gt,             20, 21),
//          new InfixOperator(TokenKind.GtEq,           20, 21),

            new InfixOperator(TokenKind.Equal2,         15, 16),
            new InfixOperator(TokenKind.NotEqual,       15, 16),

//          new InfixOperator(TokenKind.And2,           10, 11),

//          new InfixOperator(TokenKind.Or2,             5,  6),
        };

        private IExpression ParsePratt(Scanner scan, int minimumBindPower)
        {
            // pratt parsing
            // https://matklad.github.io/2020/04/13/simple-but-powerful-pratt-parsing.html

            IExpression left;

            // find prefix operator
            var prefix = (PrefixOperator)Operators.Find(x =>
                x.OperatorKind == OperatorKind.PrefixOperator && ((PrefixOperator)x).TokenKind == scan.GetTokenKind());
            if (prefix != null)
            {
                left = ParsePrefix(scan, prefix.BindPower);
            }
            else
            {
                left = ParseAtom(scan);
            }

            while (true)
            {
                // find postfix operator
                var postfix = (PostfixOperator)Operators.Find(x =>
                    x.OperatorKind == OperatorKind.PostfixOperator && ((PostfixOperator)x).TokenKind == scan.GetTokenKind());
                if (postfix != null)
                {
                    if (postfix.BindPower < minimumBindPower)
                    {
                        break;
                    }

                    left = ParsePostfix(scan, left);
                    continue;
                }

                // find infix operator
                var infix = (InfixOperator)Operators.Find(x =>
                    x.OperatorKind == OperatorKind.InfixOperator && ((InfixOperator)x).TokenKind == scan.GetTokenKind());
                if (infix != null)
                {
                    if (infix.LeftBindPower < minimumBindPower)
                    {
                        break;
                    }

                    left = ParseInfix(scan, left, infix.RightBindPower);
                    continue;
                }

                break;
            }

            return left;
        }

        private IExpression ParseAtom(Scanner scan)
        {
            throw new NotImplementedException();
        }

        private IExpression ParsePrefix(Scanner scan, int minimumBindPower)
        {
            throw new NotImplementedException();
        }

        private IExpression ParseInfix(Scanner scan, IExpression left, int minimumBindPower)
        {
            throw new NotImplementedException();
        }

        private IExpression ParsePostfix(Scanner scan, IExpression expr)
        {
            throw new NotImplementedException();
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
