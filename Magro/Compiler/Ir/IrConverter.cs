using System;
using System.Collections.Generic;

namespace Magro.Compiler
{
    internal class IrConverter
    {
        // from syake

        public IrModuleDeclaration ConvertModule(SyModuleDeclaration module)
        {
            var statements = new List<IrStatement>();
            foreach (var statement in module.Statements)
            {
                statements.Add(ConvertStatement(statement));
            }

            return new IrModuleDeclaration()
            {
                Name = module.Name,
                Statements = statements,
            };
        }

        public IrStatement ConvertStatement(SyStatement statement)
        {
            if (statement.StatementKind == SyStatementKind.VariableDeclaration)
            {
                var varDecl = (SyVariableDeclaration)statement;

                return new IrVariableDeclaration()
                {
                    Name = varDecl.Name,
                    Initializer = ConvertExpression(varDecl.Initializer),
                };
            }

            if (statement.StatementKind == SyStatementKind.AssignStatement)
            {
                var assignment = (SyAssignStatement)statement;

                return new IrAssignStatement()
                {
                    Target = ConvertExpression(assignment.Target),
                    Content = ConvertExpression(assignment.Content),
                };
            }

            throw new NotImplementedException();
        }

        public IrExpression ConvertExpression(SyExpression expression)
        {
            if (expression.ExpressionKind == SyExpressionKind.ValueExpression)
            {
                var valueExpr = (SyValueExpression)expression;

                return new IrValueExpression()
                {
                    Value = valueExpr.Value,
                    ValueKind = ConvertValueKind(valueExpr.ValueKind),
                };
            }

            if (expression.ExpressionKind == SyExpressionKind.ReferenceExpression)
            {
                var refExpr = (SyReferenceExpression)expression;

                return new IrReferenceExpression()
                {
                    Name = refExpr.Name,
                };
            }

            throw new NotImplementedException();
        }

        public IrValueKind ConvertValueKind(SyValueKind valueKind)
        {
            if (valueKind == SyValueKind.Number)
                return IrValueKind.Number;

            if (valueKind == SyValueKind.String)
                return IrValueKind.String;

            if (valueKind == SyValueKind.Boolean)
                return IrValueKind.Boolean;

            if (valueKind == SyValueKind.Null)
                return IrValueKind.Null;

            throw new NotImplementedException();
        }

        // from ikura

        public IrModuleDeclaration ConvertModule(IkModuleDeclaration module)
        {
            var statements = new List<IrStatement>();
            foreach (var statement in module.Statements)
            {
                statements.Add(ConvertStatement(statement));
            }

            return new IrModuleDeclaration()
            {
                Name = module.Name,
                Statements = statements,
            };
        }

        public IrStatement ConvertStatement(IkStatement statement)
        {
            throw new NotImplementedException();
        }
    }
}
