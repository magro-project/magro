namespace Magro.Golang.GolangAst
{
    public interface IGoStatement
    {
        StatementKind StatementKind { get; }
    }

    public interface IGoDeclaration
    {
        DeclarationKind DeclarationKind { get; }

        string Name { get; set; }
    }

    public interface IGoExpression
    {
        ExpressionKind ExpressionKind { get; }
    }

    public enum StatementKind
    {
        
    }

    public enum DeclarationKind
    {
        
    }

    public enum ExpressionKind
    {
        ValueExpression,
        ReferenceExpression,
        RelationalOperator,
        LogicOperator,
        MathOperator,
    }
}
