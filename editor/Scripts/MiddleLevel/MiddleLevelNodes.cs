using System.Collections.Generic;

namespace Magro.Scripts.MiddleLevel
{
    internal interface IStatement
    {
        StatementKind StatementKind { get; }
    }

    internal interface IDeclaration
    {
        DeclarationKind DeclarationKind { get; }

        string Name { get; set; }
    }

    internal interface IExpression
    {
        ExpressionKind ExpressionKind { get; }
    }

    internal enum StatementKind
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

    internal enum DeclarationKind
    {
        ModuleDeclaration,
        FunctionDeclaration,
        VariableDeclaration,
    }

    internal enum ExpressionKind
    {
        ValueExpression,
        ReferenceExpression,
        IndexExpression,
        CallExpression,
        NotOperator,
        LogicOperator,
        MathOperator,
    }

    internal enum LogicOperatorKind
    {
        Equal,      // Left == Right
        NotEqual,   // Left != Right
    }

    internal enum MathOperatorKind
    {
        Add, // Left + Right
        Sub, // Left - Right
        Mul, // Left * Right
        Div, // Left / Right
        Rem, // Left % Right
    }

    internal enum ValueKind
    {
        Number,
        String,
        Boolean,
        Object,
    }

    internal class ModuleDeclaration : IDeclaration
    {
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.ModuleDeclaration;

        public string Name { get; set; }
        public List<IDeclaration> Declarations { get; set; } = new List<IDeclaration>();
        public List<IStatement> Statements { get; set; }
    }

    // var A = B;
    internal class VariableDeclaration : IStatement, IDeclaration
    {
        public StatementKind StatementKind { get; } = StatementKind.VariableDeclaration;
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.VariableDeclaration;

        public string Name { get; set; }
        public IExpression Initializer { get; set; }
    }

    // function A(...B) { C }
    internal class FunctionDeclaration : IStatement, IDeclaration
    {
        public StatementKind StatementKind { get; } = StatementKind.FunctionDeclaration;
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.FunctionDeclaration;

        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public Block FunctionBlock { get; set; }
    }

    // A = B;
    internal class AssignStatement : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.AssignStatement;

        public IExpression Target { get; set; }
        public IExpression Content { get; set; }
    }

    // A++;
    internal class IncrementStatement : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.IncrementStatement;

        public IExpression Target { get; set; }
    }

    // A--;
    internal class DecrementStatement : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.DecrementStatement;

        public IExpression Target { get; set; }
    }

    // if A { B } else { C }
    internal class IfStatement : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.IfStatement;

        public IExpression Condition { get; set; }
        public Block ThenBlock { get; set; }
        public Block ElseBlock { get; set; }
    }

    // { }
    internal class Block : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.Block;

        public List<IDeclaration> Declarations { get; set; } = new List<IDeclaration>();
        public List<IStatement> Statements { get; set; }
    }

    // A;
    internal class ExpressionStatement : IStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ExpressionStatement;

        public IExpression Expression { get; set; }
    }

    // Value
    internal class ValueExpression : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.ValueExpression;
        public ValueKind ValueKind { get; set; }

        public object Value { get; set; }
    }

    // Name
    internal class ReferenceExpression : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.ReferenceExpression;

        public string Name { get; set; }
        public IDeclaration ResolvedDeclaration { get; set; }
    }

    // Target[...Indexes]
    internal class IndexExpression : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.IndexExpression;

        public IExpression Target { get; set; }
        public List<IExpression> Indexes { get; set; }
    }

    // Target(...Arguments)
    internal class CallExpression : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.CallExpression;

        public IExpression Target { get; set; }
        public List<IExpression> Arguments { get; set; }
    }

    internal class NotOperator : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.NotOperator;

        public IExpression Left { get; set; }
    }

    internal class LogicOperator : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.LogicOperator;
        public LogicOperatorKind LogicOperationKind { get; set; }

        public IExpression Left { get; set; }
        public IExpression Right { get; set; }
    }

    internal class MathOperator : IExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.MathOperator;
        public MathOperatorKind MathOperationKind { get; set; }

        public IExpression Left { get; set; }
        public IExpression Right { get; set; }
    }
}
