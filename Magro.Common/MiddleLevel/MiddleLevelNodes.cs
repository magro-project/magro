using System.Collections.Generic;

namespace Magro.Common.MiddleLevel
{
    public interface IStatement
    {
        StatementKind StatementKind { get; }
    }

    public interface IDeclaration
    {
        DeclarationKind DeclarationKind { get; }

        string Name { get; set; }
    }

    public interface IExpression
    {
        ExpressionKind ExpressionKind { get; }
    }

    public enum StatementKind
    {
        VariableDeclaration,
        FunctionDeclaration,
        AssignStatement,
        IncrementStatement,
        DecrementStatement,
        IfStatement,
        Block,
        ExpressionStatement,
    }

    public enum DeclarationKind
    {
        ModuleDeclaration,
        FunctionDeclaration,
        VariableDeclaration,
    }

    public enum ExpressionKind
    {
        ValueExpression,
        ReferenceExpression,
        IndexExpression,
        CallExpression,
        NotOperator,
        RelationalOperator,
        LogicOperator,
        MathOperator,
    }

    public enum RelationalOperatorKind
    {
        Equal,      // Left == Right
        NotEqual,   // Left != Right
        Gt,         // Left > Right
        Lt,         // Left < Right
        GtEq,       // Left >= Right
        LtEq,       // Left <= Right
    }

    public enum LogicOperatorKind
    {
        And,    // Left && Right
        Or,     // Left || Right
    }

    public enum MathOperatorKind
    {
        Add,    // Left + Right
        Sub,    // Left - Right
        Mul,    // Left * Right
        Div,    // Left / Right
        Rem,    // Left % Right
    }

    public enum ValueKind
    {
        Null,
        Number,
        String,
        Boolean,
        Object,
    }

    public class ModuleDeclaration : IDeclaration
    {
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.ModuleDeclaration;

        // semantics
        public List<IDeclaration> Declarations { get; set; } = new List<IDeclaration>();

        public string Name { get; set; }
        public List<IStatement> Statements { get; set; }
    }

    // var A = B;
    public class VariableDeclaration : IStatement, IDeclaration
    {
        public StatementKind StatementKind { get; } = StatementKind.VariableDeclaration;
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.VariableDeclaration;

        public string Name { get; set; }
        public IExpression Initializer { get; set; }
    }

    // function A(...B) { C }
    public class FunctionDeclaration : IStatement, IDeclaration
    {
        public StatementKind StatementKind { get; } = StatementKind.FunctionDeclaration;
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.FunctionDeclaration;

        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public Block FunctionBlock { get; set; }
    }

    // A = B;
    public class AssignStatement : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.AssignStatement;

        public IExpression Target { get; set; }
        public IExpression Content { get; set; }
    }

    // A++;
    public class IncrementStatement : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.IncrementStatement;

        public IExpression Target { get; set; }
    }

    // A--;
    public class DecrementStatement : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.DecrementStatement;

        public IExpression Target { get; set; }
    }

    // if A { B } else { C }
    public class IfStatement : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.IfStatement;

        public IExpression Condition { get; set; }
        public Block ThenBlock { get; set; }
        public Block ElseBlock { get; set; }
    }

    // { }
    public class Block : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.Block;

        // semantics
        public List<IDeclaration> Declarations { get; set; } = new List<IDeclaration>();

        public List<IStatement> Statements { get; set; }
    }

    // A;
    public class ExpressionStatement : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ExpressionStatement;

        public IExpression Expression { get; set; }
    }

    // Value
    public class ValueExpression : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.ValueExpression;

        public ValueKind ValueKind { get; set; }
        public object Value { get; set; }
    }

    // Name
    public class ReferenceExpression : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.ReferenceExpression;

        // semantics
        public IDeclaration ResolvedDeclaration { get; set; }

        public string Name { get; set; }
    }

    // Target[...Indexes]
    public class IndexExpression : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.IndexExpression;

        public IExpression Target { get; set; }
        public List<IExpression> Indexes { get; set; }
    }

    // Target(...Arguments)
    public class CallExpression : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.CallExpression;

        public IExpression Target { get; set; }
        public List<IExpression> Arguments { get; set; }
    }

    public class NotOperator : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.NotOperator;

        public IExpression Left { get; set; }
    }

    public class RelationalOperator : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.RelationalOperator;

        public RelationalOperatorKind RelationalOperatorKind { get; set; }
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }
    }

    public class LogicOperator : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.LogicOperator;

        public LogicOperatorKind LogicOperatorKind { get; set; }
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }
    }

    public class MathOperator : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.MathOperator;

        public MathOperatorKind MathOperatorKind { get; set; }
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }
    }
}
