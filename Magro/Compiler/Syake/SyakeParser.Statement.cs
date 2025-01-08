using System;
using System.Collections.Generic;

namespace Magro.Compiler
{
    internal partial class SyakeParser
    {
        public List<SyStatement> ParseStatement(SyakeScanner scanner)
        {
            if (scanner.Is("function"))
            {
                scanner.Next();
                scanner.Expect(TokenKind.Word);
                var name = scanner.GetTokenContent();
                scanner.Next();
                var parameters = ParseParameters(scanner);
                var block = ParseBlock(scanner);

                return new List<SyStatement>()
                {
                    new SyFunctionDeclaration()
                    {
                        Name = name,
                        Parameters = parameters,
                        FunctionBlock = block,
                    }
                };
            }

            if (scanner.Is("var"))
            {
                scanner.Next();

                var statements = new List<SyStatement>();
                while (true)
                {
                    scanner.Expect(TokenKind.Word);
                    var name = scanner.GetTokenContent();
                    scanner.Next();

                    SyExpression initializer = null;
                    if (scanner.Is(TokenKind.Equal))
                    {
                        scanner.Next();
                        initializer = ParseExpression(scanner);
                    }

                    statements.Add(new SyVariableDeclaration()
                    {
                        Name = name,
                        Initializer = initializer,
                    });

                    // 「,」で区切って複数の変数を宣言できる
                    if (scanner.Is(TokenKind.Comma))
                    {
                        scanner.Next();
                    }
                    else
                    {
                        break;
                    }
                }

                scanner.Expect(TokenKind.SemiCollon);
                scanner.Next();

                return statements;
            }

            if (scanner.Is("if"))
            {
                scanner.Next();
                scanner.Expect(TokenKind.OpenParen);
                scanner.Next();
                var condition = ParseExpression(scanner);
                scanner.Expect(TokenKind.CloseParen);
                scanner.Next();

                SyBlock thenBlock;
                if (scanner.Is(TokenKind.OpenBrace))
                {
                    thenBlock = ParseBlock(scanner);
                }
                else
                {
                    var statements = ParseStatement(scanner);
                    thenBlock = new SyBlock()
                    {
                        Statements = statements,
                    };
                }

                SyBlock elseBlock = null;
                if (scanner.Is("else"))
                {
                    scanner.Next();

                    if (scanner.Is(TokenKind.OpenBrace))
                    {
                        elseBlock = ParseBlock(scanner);
                    }
                    else
                    {
                        var statements = ParseStatement(scanner);
                        elseBlock = new SyBlock()
                        {
                            Statements = statements,
                        };
                    }
                }

                return new List<SyStatement>()
                {
                    new SyIfStatement()
                    {
                        Condition = condition,
                        ThenBlock = thenBlock,
                        ElseBlock = elseBlock,
                    }
                };
            }

            if (scanner.Is("while"))
            {
                scanner.Next();
                scanner.Expect(TokenKind.OpenParen);
                scanner.Next();
                var condition = ParseExpression(scanner);
                scanner.Expect(TokenKind.CloseParen);
                scanner.Next();

                SyBlock loopBlock;
                if (scanner.Is(TokenKind.OpenBrace))
                {
                    loopBlock = ParseBlock(scanner);
                }
                else
                {
                    var statements = ParseStatement(scanner);
                    loopBlock = new SyBlock()
                    {
                        Statements = statements,
                    };
                }

                return new List<SyStatement>()
                {
                    new SyWhileStatement()
                    {
                        Condition = condition,
                        LoopBlock = loopBlock,
                    }
                };
            }

            if (scanner.Is("for"))
            {
                scanner.Next();
                scanner.Expect(TokenKind.OpenParen);
                scanner.Next();
                scanner.Expect("var");
                scanner.Next();
                scanner.Expect(TokenKind.Word);
                var name = scanner.GetTokenContent();
                scanner.Next();
                scanner.Expect("in");
                scanner.Next();
                var iterable = ParseExpression(scanner);
                scanner.Expect(TokenKind.CloseParen);
                scanner.Next();

                SyBlock loopBlock;
                if (scanner.Is(TokenKind.OpenBrace))
                {
                    loopBlock = ParseBlock(scanner);
                }
                else
                {
                    var statements = ParseStatement(scanner);
                    loopBlock = new SyBlock()
                    {
                        Statements = statements,
                    };
                }

                return new List<SyStatement>()
                {
                    new SyForStatement()
                    {
                        VariableName = name,
                        Iterable = iterable,
                        LoopBlock = loopBlock,
                    }
                };
            }

            if (scanner.Is("break"))
            {
                scanner.Next();
                scanner.Expect(TokenKind.SemiCollon);
                scanner.Next();

                return new List<SyStatement>()
                {
                    new SyBreakStatement()
                };
            }

            if (scanner.Is("continue"))
            {
                scanner.Next();
                scanner.Expect(TokenKind.SemiCollon);
                scanner.Next();

                return new List<SyStatement>()
                {
                    new SyContinueStatement()
                };
            }

            if (scanner.Is("return"))
            {
                scanner.Next();

                SyExpression value = null;
                if (!scanner.Is(TokenKind.SemiCollon))
                {
                    value = ParseExpression(scanner);
                }

                scanner.Expect(TokenKind.SemiCollon);
                scanner.Next();

                return new List<SyStatement>()
                {
                    new SyReturnStatement()
                    {
                        Value = value,
                    }
                };
            }

            var expression = ParseExpression(scanner);

            // expression statement
            if (scanner.Is(TokenKind.SemiCollon))
            {
                scanner.Next();

                return new List<SyStatement>()
                {
                    new SyExpressionStatement()
                    {
                        Expression = expression,
                    }
                };
            }

            // assign statement
            if (scanner.Is(TokenKind.Equal))
            {
                scanner.Next();
                var right = ParseExpression(scanner);
                scanner.Expect(TokenKind.SemiCollon);
                scanner.Next();

                return new List<SyStatement>()
                {
                    new SyAssignStatement()
                    {
                        Target = expression,
                        Content = right,
                    }
                };
            }

            // increment statement
            if (scanner.Is(TokenKind.Plus2))
            {
                scanner.Next();
                scanner.Expect(TokenKind.SemiCollon);
                scanner.Next();

                return new List<SyStatement>()
                {
                    new SyIncrementStatement()
                    {
                        Target = expression,
                    }
                };
            }

            // decrement statement
            if (scanner.Is(TokenKind.Minus2))
            {
                scanner.Next();
                scanner.Expect(TokenKind.SemiCollon);
                scanner.Next();

                return new List<SyStatement>()
                {
                    new SyDecrementStatement()
                    {
                        Target = expression,
                    }
                };
            }

            throw new ApplicationException("Unexpected token " + scanner.GetToken());
        }

        public List<string> ParseParameters(SyakeScanner scanner)
        {
            scanner.Expect(TokenKind.OpenParen);
            scanner.Next();

            var parameters = new List<string>();

            while (!scanner.Is(TokenKind.CloseParen))
            {
                scanner.Expect(TokenKind.Word);
                parameters.Add(scanner.GetTokenContent());
                scanner.Next();

                if (scanner.Is(TokenKind.Comma))
                {
                    scanner.Next();
                }
            }

            scanner.Expect(TokenKind.CloseParen);
            scanner.Next();

            return parameters;
        }

        public SyBlock ParseBlock(SyakeScanner scanner)
        {
            scanner.Expect(TokenKind.OpenBrace);
            scanner.Next();

            var statements = new List<SyStatement>();
            while (!scanner.Is(TokenKind.CloseBrace))
            {
                statements.AddRange(ParseStatement(scanner));
            }

            scanner.Expect(TokenKind.CloseBrace);
            scanner.Next();

            return new SyBlock()
            {
                Statements = statements,
            };
        }
    }
}
