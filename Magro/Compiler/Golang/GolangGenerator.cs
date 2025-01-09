namespace Magro.Compiler
{
    internal class GolangGenerator
    {
        public void Generate(CodeWriter writer, IrModuleDeclaration module)
        {
            writer.WriteLine("package main");
            writer.WriteLine();
            writer.WriteLine("func ScriptMain() {");

            writer.EnterIndent();
            foreach (var statement in module.Statements)
            {
                EmitStatement(writer, statement);
            }
            writer.LeaveIndent();

            writer.WriteLine("}");
        }

        public void EmitStatement(CodeWriter writer, IrStatement statement)
        {
            if (statement.StatementKind == IrStatementKind.VariableDeclaration)
            {
                var varDecl = (IrVariableDeclaration)statement;
                writer.WriteIndent();
                writer.Write(varDecl.Name);
                writer.Write(" := ");
                EmitExpression(writer, varDecl.Initializer);
                writer.WriteLine();
            }

            if (statement.StatementKind == IrStatementKind.AssignStatement)
            {
                var assign = (IrAssignStatement)statement;
                writer.WriteIndent();
                EmitExpression(writer, assign.Target);
                writer.Write(" = ");
                EmitExpression(writer, assign.Content);
                writer.WriteLine();
            }
        }

        public void EmitExpression(CodeWriter writer, IrExpression expression)
        {
            if (expression.ExpressionKind == IrExpressionKind.ValueExpression)
            {
                var valueExpr = (IrValueExpression)expression;
                writer.Write(valueExpr.Value.ToString());
            }

            if (expression.ExpressionKind == IrExpressionKind.ReferenceExpression)
            {
                var refExpr = (IrReferenceExpression)expression;
                writer.Write(refExpr.Name);
            }
        }
    }
}
