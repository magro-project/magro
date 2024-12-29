using System;
using System.Collections.Generic;

namespace Magro.Syake.Syntax
{
    internal partial class SyakeParser
    {
        public List<ISyStatement> ParseStatement(SyakeTokenReader reader)
        {
            if (reader.Is("function"))
            {
                reader.Next();
                reader.Expect(TokenKind.Word);
                var name = reader.GetTokenContent();
                reader.Next();
                var parameters = ParseParameters(reader);
                var block = ParseBlock(reader);

                return new List<ISyStatement>()
                {
                    new SyFunctionDeclaration()
                    {
                        Name = name,
                        Parameters = parameters,
                        FunctionBlock = block,
                    }
                };
            }

            if (reader.Is("var"))
            {
                reader.Next();

                var statements = new List<ISyStatement>();
                while (true)
                {
                    reader.Expect(TokenKind.Word);
                    var name = reader.GetTokenContent();
                    reader.Next();

                    ISyExpression initializer = null;
                    if (reader.Is(TokenKind.Equal))
                    {
                        reader.Next();
                        initializer = ParseExpression(reader);
                    }

                    statements.Add(new SyVariableDeclaration()
                    {
                        Name = name,
                        Initializer = initializer,
                    });

                    // 「,」で区切って複数の変数を宣言できる
                    if (reader.Is(TokenKind.Comma))
                    {
                        reader.Next();
                    }
                    else
                    {
                        break;
                    }
                }

                reader.Expect(TokenKind.SemiCollon);
                reader.Next();

                return statements;
            }

            if (reader.Is("if"))
            {
                reader.Next();
                reader.Expect(TokenKind.OpenParen);
                reader.Next();
                var condition = ParseExpression(reader);
                reader.Expect(TokenKind.CloseParen);
                reader.Next();

                SyBlock thenBlock;
                if (reader.Is(TokenKind.OpenBrace))
                {
                    thenBlock = ParseBlock(reader);
                }
                else
                {
                    var statements = ParseStatement(reader);
                    thenBlock = new SyBlock()
                    {
                        Statements = statements,
                    };
                }

                SyBlock elseBlock = null;
                if (reader.Is("else"))
                {
                    reader.Next();

                    if (reader.Is(TokenKind.OpenBrace))
                    {
                        elseBlock = ParseBlock(reader);
                    }
                    else
                    {
                        var statements = ParseStatement(reader);
                        elseBlock = new SyBlock()
                        {
                            Statements = statements,
                        };
                    }
                }

                return new List<ISyStatement>()
                {
                    new SyIfStatement()
                    {
                        Condition = condition,
                        ThenBlock = thenBlock,
                        ElseBlock = elseBlock,
                    }
                };
            }

            if (reader.Is("while"))
            {
                reader.Next();
                reader.Expect(TokenKind.OpenParen);
                reader.Next();
                var condition = ParseExpression(reader);
                reader.Expect(TokenKind.CloseParen);
                reader.Next();

                SyBlock loopBlock;
                if (reader.Is(TokenKind.OpenBrace))
                {
                    loopBlock = ParseBlock(reader);
                }
                else
                {
                    var statements = ParseStatement(reader);
                    loopBlock = new SyBlock()
                    {
                        Statements = statements,
                    };
                }

                return new List<ISyStatement>()
                {
                    new SyWhileStatement()
                    {
                        Condition = condition,
                        LoopBlock = loopBlock,
                    }
                };
            }

            if (reader.Is("for"))
            {
                reader.Next();
                reader.Expect(TokenKind.OpenParen);
                reader.Next();
                reader.Expect("var");
                reader.Next();
                reader.Expect(TokenKind.Word);
                var name = reader.GetTokenContent();
                reader.Next();
                reader.Expect("in");
                reader.Next();
                var iterable = ParseExpression(reader);
                reader.Expect(TokenKind.CloseParen);
                reader.Next();

                SyBlock loopBlock;
                if (reader.Is(TokenKind.OpenBrace))
                {
                    loopBlock = ParseBlock(reader);
                }
                else
                {
                    var statements = ParseStatement(reader);
                    loopBlock = new SyBlock()
                    {
                        Statements = statements,
                    };
                }

                return new List<ISyStatement>()
                {
                    new SyForStatement()
                    {
                        VariableName = name,
                        Iterable = iterable,
                        LoopBlock = loopBlock,
                    }
                };
            }

            if (reader.Is("break"))
            {
                reader.Next();
                reader.Expect(TokenKind.SemiCollon);
                reader.Next();

                return new List<ISyStatement>()
                {
                    new SyBreakStatement()
                };
            }

            if (reader.Is("continue"))
            {
                reader.Next();
                reader.Expect(TokenKind.SemiCollon);
                reader.Next();

                return new List<ISyStatement>()
                {
                    new SyContinueStatement()
                };
            }

            if (reader.Is("return"))
            {
                reader.Next();

                ISyExpression value = null;
                if (!reader.Is(TokenKind.SemiCollon))
                {
                    value = ParseExpression(reader);
                }

                reader.Expect(TokenKind.SemiCollon);
                reader.Next();

                return new List<ISyStatement>()
                {
                    new SyReturnStatement()
                    {
                        Value = value,
                    }
                };
            }

            var expression = ParseExpression(reader);

            // expression statement
            if (reader.Is(TokenKind.SemiCollon))
            {
                reader.Next();

                return new List<ISyStatement>()
                {
                    new SyExpressionStatement()
                    {
                        Expression = expression,
                    }
                };
            }

            // assign statement
            if (reader.Is(TokenKind.Equal))
            {
                reader.Next();
                var right = ParseExpression(reader);
                reader.Expect(TokenKind.SemiCollon);
                reader.Next();

                return new List<ISyStatement>()
                {
                    new SyAssignStatement()
                    {
                        Target = expression,
                        Content = right,
                    }
                };
            }

            // increment statement
            if (reader.Is(TokenKind.Plus2))
            {
                reader.Next();
                reader.Expect(TokenKind.SemiCollon);
                reader.Next();

                return new List<ISyStatement>()
                {
                    new SyIncrementStatement()
                    {
                        Target = expression,
                    }
                };
            }

            // decrement statement
            if (reader.Is(TokenKind.Minus2))
            {
                reader.Next();
                reader.Expect(TokenKind.SemiCollon);
                reader.Next();

                return new List<ISyStatement>()
                {
                    new SyDecrementStatement()
                    {
                        Target = expression,
                    }
                };
            }

            throw new ApplicationException("Unexpected token " + reader.GetToken());
        }

        public List<string> ParseParameters(SyakeTokenReader reader)
        {
            reader.Expect(TokenKind.OpenParen);
            reader.Next();

            var parameters = new List<string>();

            while (!reader.Is(TokenKind.CloseParen))
            {
                reader.Expect(TokenKind.Word);
                parameters.Add(reader.GetTokenContent());
                reader.Next();

                if (reader.Is(TokenKind.Comma))
                {
                    reader.Next();
                }
            }

            reader.Expect(TokenKind.CloseParen);
            reader.Next();

            return parameters;
        }

        public SyBlock ParseBlock(SyakeTokenReader reader)
        {
            reader.Expect(TokenKind.OpenBrace);
            reader.Next();

            var statements = new List<ISyStatement>();
            while (!reader.Is(TokenKind.CloseBrace))
            {
                statements.AddRange(ParseStatement(reader));
            }

            reader.Expect(TokenKind.CloseBrace);
            reader.Next();

            return new SyBlock()
            {
                Statements = statements,
            };
        }
    }
}
