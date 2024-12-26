using Magro.Common.MiddleLevel;
using System;
using System.Collections.Generic;

namespace Magro.Syake.Syntax
{
    internal partial class Parser
    {
        public List<IStatement> ParseStatement(Scanner scan)
        {
            if (scan.Is("function"))
            {
                scan.Next();
                scan.Expect(TokenKind.Word);
                var name = scan.GetTokenContent();
                scan.Next();
                var parameters = ParseParameters(scan);
                var block = ParseBlock(scan);

                return new List<IStatement>()
                {
                    new FunctionDeclaration()
                    {
                        Name = name,
                        Parameters = parameters,
                        FunctionBlock = block,
                    }
                };
            }

            if (scan.Is("var"))
            {
                scan.Next();

                var statements = new List<IStatement>();
                while (true)
                {
                    scan.Expect(TokenKind.Word);
                    var name = scan.GetTokenContent();
                    scan.Next();

                    IExpression initializer = null;
                    if (scan.Is(TokenKind.Equal))
                    {
                        scan.Next();
                        initializer = ParseExpression(scan);
                    }

                    statements.Add(new VariableDeclaration()
                    {
                        Name = name,
                        Initializer = initializer,
                    });

                    // 「,」で区切って複数の変数を宣言できる
                    if (scan.Is(TokenKind.Comma))
                    {
                        scan.Next();
                    }
                    else
                    {
                        break;
                    }
                }

                scan.Expect(TokenKind.SemiCollon);
                scan.Next();

                return statements;
            }

            if (scan.Is("if"))
            {
                scan.Next();
                scan.Expect(TokenKind.OpenParen);
                scan.Next();
                var condition = ParseExpression(scan);
                scan.Expect(TokenKind.CloseParen);
                scan.Next();

                Block thenBlock;
                if (scan.Is(TokenKind.OpenBrace))
                {
                    thenBlock = ParseBlock(scan);
                }
                else
                {
                    var statements = ParseStatement(scan);
                    thenBlock = new Block()
                    {
                        Statements = statements,
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
                        var statements = ParseStatement(scan);
                        elseBlock = new Block()
                        {
                            Statements = statements,
                        };
                    }
                }

                return new List<IStatement>()
                {
                    new IfStatement()
                    {
                        Condition = condition,
                        ThenBlock = thenBlock,
                        ElseBlock = elseBlock,
                    }
                };
            }

            if (scan.Is("while"))
            {
                scan.Next();
                scan.Expect(TokenKind.OpenParen);
                scan.Next();
                var condition = ParseExpression(scan);
                scan.Expect(TokenKind.CloseParen);
                scan.Next();

                Block loopBlock;
                if (scan.Is(TokenKind.OpenBrace))
                {
                    loopBlock = ParseBlock(scan);
                }
                else
                {
                    var statements = ParseStatement(scan);
                    loopBlock = new Block()
                    {
                        Statements = statements,
                    };
                }

                return new List<IStatement>()
                {
                    new WhileStatement()
                    {
                        Condition = condition,
                        LoopBlock = loopBlock,
                    }
                };
            }

            if (scan.Is("for"))
            {
                scan.Next();
                scan.Expect(TokenKind.OpenParen);
                scan.Next();
                scan.Expect("var");
                scan.Next();
                scan.Expect(TokenKind.Word);
                var name = scan.GetTokenContent();
                scan.Next();
                scan.Expect("in");
                scan.Next();
                var iterable = ParseExpression(scan);
                scan.Expect(TokenKind.CloseParen);
                scan.Next();

                Block loopBlock;
                if (scan.Is(TokenKind.OpenBrace))
                {
                    loopBlock = ParseBlock(scan);
                }
                else
                {
                    var statements = ParseStatement(scan);
                    loopBlock = new Block()
                    {
                        Statements = statements,
                    };
                }

                return new List<IStatement>()
                {
                    new ForStatement()
                    {
                        VariableName = name,
                        Iterable = iterable,
                        LoopBlock = loopBlock,
                    }
                };
            }

            if (scan.Is("break"))
            {
                scan.Next();
                scan.Expect(TokenKind.SemiCollon);
                scan.Next();

                return new List<IStatement>()
                {
                    new BreakStatement()
                };
            }

            if (scan.Is("continue"))
            {
                scan.Next();
                scan.Expect(TokenKind.SemiCollon);
                scan.Next();

                return new List<IStatement>()
                {
                    new ContinueStatement()
                };
            }

            if (scan.Is("return"))
            {
                scan.Next();

                IExpression value = null;
                if (!scan.Is(TokenKind.SemiCollon))
                {
                    value = ParseExpression(scan);
                }

                scan.Expect(TokenKind.SemiCollon);
                scan.Next();

                return new List<IStatement>()
                {
                    new ReturnStatement()
                    {
                        Value = value,
                    }
                };
            }

            var expression = ParseExpression(scan);

            // expression statement
            if (scan.Is(TokenKind.SemiCollon))
            {
                scan.Next();

                return new List<IStatement>()
                {
                    new ExpressionStatement()
                    {
                        Expression = expression,
                    }
                };
            }

            // assign statement
            if (scan.Is(TokenKind.Equal))
            {
                scan.Next();
                var right = ParseExpression(scan);
                scan.Expect(TokenKind.SemiCollon);
                scan.Next();

                return new List<IStatement>()
                {
                    new AssignStatement()
                    {
                        Target = expression,
                        Content = right,
                    }
                };
            }

            // increment statement
            if (scan.Is(TokenKind.Plus2))
            {
                scan.Next();
                scan.Expect(TokenKind.SemiCollon);
                scan.Next();

                return new List<IStatement>()
                {
                    new IncrementStatement()
                    {
                        Target = expression,
                    }
                };
            }

            // decrement statement
            if (scan.Is(TokenKind.Minus2))
            {
                scan.Next();
                scan.Expect(TokenKind.SemiCollon);
                scan.Next();

                return new List<IStatement>()
                {
                    new DecrementStatement()
                    {
                        Target = expression,
                    }
                };
            }

            throw new ApplicationException("Unexpected token " + scan.GetToken());
        }

        public List<string> ParseParameters(Scanner scan)
        {
            scan.Expect(TokenKind.OpenParen);
            scan.Next();

            var parameters = new List<string>();

            while (!scan.Is(TokenKind.CloseParen))
            {
                scan.Expect(TokenKind.Word);
                parameters.Add(scan.GetTokenContent());
                scan.Next();

                if (scan.Is(TokenKind.Comma))
                {
                    scan.Next();
                }
            }

            scan.Expect(TokenKind.CloseParen);
            scan.Next();

            return parameters;
        }

        public Block ParseBlock(Scanner scan)
        {
            scan.Expect(TokenKind.OpenBrace);
            scan.Next();

            var statements = new List<IStatement>();
            while (!scan.Is(TokenKind.CloseBrace))
            {
                statements.AddRange(ParseStatement(scan));
            }

            scan.Expect(TokenKind.CloseBrace);
            scan.Next();

            return new Block()
            {
                Statements = statements,
            };
        }
    }
}
