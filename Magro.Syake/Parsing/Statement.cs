﻿using Magro.Common.MiddleLevel;
using System;
using System.Collections.Generic;

namespace Magro.Syake.Parsing
{
    internal partial class Parser
    {
        public IStatement ParseStatement(Scanner scan)
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

            if (scan.Is("if"))
            {
                scan.Next();
                scan.Expect(TokenKind.OpenParen);
                var condition = ParseExpression(scan);
                scan.Expect(TokenKind.CloseParen);

                Block thenBlock;
                if (scan.Is(TokenKind.OpenBrace))
                {
                    thenBlock = ParseBlock(scan);
                }
                else
                {
                    var statement = ParseStatement(scan);
                    thenBlock = new Block()
                    {
                        Statements = new List<IStatement>() { statement },
                    };
                }

                Block elseBlock = null;
                if (scan.Is("else"))
                {
                    scan.Next();

                    if (scan.Is(TokenKind.OpenBrace))
                    {
                        elseBlock = ParseBlock(scan);
                    }
                    else
                    {
                        var statement = ParseStatement(scan);
                        elseBlock = new Block()
                        {
                            Statements = new List<IStatement>() { statement },
                        };
                    }
                }

                return new IfStatement()
                {
                    Condition = condition,
                    ThenBlock = thenBlock,
                    ElseBlock = elseBlock,
                };
            }

            var expression = ParseExpression(scan);

            // expression statement
            if (scan.Is(TokenKind.SemiCollon))
            {
                scan.Next();

                return new ExpressionStatement()
                {
                    Expression = expression,
                };
            }

            // assign statement
            if (scan.Is(TokenKind.Equal))
            {
                scan.Next();
                var right = ParseExpression(scan);
                scan.Expect(TokenKind.SemiCollon);

                return new AssignStatement()
                {
                    Target = expression,
                    Content = right,
                };
            }

            // increment statement
            if (scan.Is(TokenKind.Plus2))
            {
                scan.Next();
                scan.Expect(TokenKind.SemiCollon);

                return new IncrementStatement()
                {
                    Target = expression,
                };
            }

            // decrement statement
            if (scan.Is(TokenKind.Minus2))
            {
                scan.Next();
                scan.Expect(TokenKind.SemiCollon);

                return new DecrementStatement()
                {
                    Target = expression,
                };
            }

            throw new ApplicationException("Unexpected token");
        }

        public List<string> ParseParameters(Scanner scan)
        {
            scan.Expect(TokenKind.OpenParen);

            var parameters = new List<string>();

            while (!scan.Is(TokenKind.CloseParen))
            {
                scan.Expect(TokenKind.Word);
                parameters.Add((string)scan.GetTokenContent());

                if (scan.Is(TokenKind.Comma))
                {
                    scan.Next();
                }
            }

            scan.Expect(TokenKind.CloseParen);

            return parameters;
        }

        public Block ParseBlock(Scanner scan)
        {
            scan.Expect(TokenKind.OpenBrace);

            var statements = new List<IStatement>();
            while (!scan.Is(TokenKind.CloseBrace))
            {
                statements.Add(ParseStatement(scan));
            }

            scan.Expect(TokenKind.CloseBrace);

            return new Block()
            {
                Statements = statements,
            };
        }
    }
}