using System.Collections.Generic;

namespace Magro.Syake.Syntax
{
    public interface ISyStatement
    {
        StatementKind StatementKind { get; }
    }

    public interface ISyDeclaration
    {
        DeclarationKind DeclarationKind { get; }

        string Name { get; set; }
    }

    public interface ISyExpression
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
        WhileStatement,
        ForStatement,
        BreakStatement,
        ContinueStatement,
        ReturnStatement,
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
        MemberAccessExpression,
        IndexAccessExpression,
        CallFuncExpression,
        NotOperator,
        SignExpression,
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

    public enum SignKind
    {
        Positive,
        Negative,
    }

    public class SyModuleDeclaration : ISyDeclaration
    {
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.ModuleDeclaration;

        // semantics
        public List<ISyDeclaration> Declarations { get; set; } = new List<ISyDeclaration>();

        public string Name { get; set; }
        public List<ISyStatement> Statements { get; set; }
    }

    // var A = B;
    public class SyVariableDeclaration : ISyStatement, ISyDeclaration
    {
        public StatementKind StatementKind { get; } = StatementKind.VariableDeclaration;
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.VariableDeclaration;

        public string Name { get; set; }
        public ISyExpression Initializer { get; set; }
    }

    // function A(...B) { C }
    public class SyFunctionDeclaration : ISyStatement, ISyDeclaration
    {
        public StatementKind StatementKind { get; } = StatementKind.FunctionDeclaration;
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.FunctionDeclaration;

        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public SyBlock FunctionBlock { get; set; }
    }

    // A = B;
    public class SyAssignStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.AssignStatement;

        public ISyExpression Target { get; set; }
        public ISyExpression Content { get; set; }
    }

    // A++;
    public class SyIncrementStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.IncrementStatement;

        public ISyExpression Target { get; set; }
    }

    // A--;
    public class SyDecrementStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.DecrementStatement;

        public ISyExpression Target { get; set; }
    }

    // if (A) { B } else { C }
    public class SyIfStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.IfStatement;

        public ISyExpression Condition { get; set; }
        public SyBlock ThenBlock { get; set; }
        public SyBlock ElseBlock { get; set; }
    }

    // while (A) { B }
    public class SyWhileStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.WhileStatement;

        public ISyExpression Condition { get; set; }
        public SyBlock LoopBlock { get; set; }
    }

    // for (var A in B) { C }
    public class SyForStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ForStatement;

        public string VariableName { get; set; }
        public ISyExpression Iterable { get; set; }
        public SyBlock LoopBlock { get; set; }
    }

    public class SyBreakStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.BreakStatement;
    }

    public class SyContinueStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ContinueStatement;
    }

    public class SyReturnStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ReturnStatement;

        public bool HasValue => Value != null;
        public ISyExpression Value { get; set; }
    }

    // { }
    public class SyBlock : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.Block;

        // semantics
        public List<ISyDeclaration> Declarations { get; set; } = new List<ISyDeclaration>();

        public List<ISyStatement> Statements { get; set; }
    }

    // A;
    public class SyExpressionStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ExpressionStatement;

        public ISyExpression Expression { get; set; }
    }

    // Value
    public class SyValueExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.ValueExpression;

        public ValueKind ValueKind { get; set; }
        public object Value { get; set; }
    }

    // Name
    public class SyReferenceExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.ReferenceExpression;

        // semantics
        public ISyDeclaration ResolvedDeclaration { get; set; }

        public string Name { get; set; }
    }

    // Target.MemberName
    public class SyMemberAccessExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.MemberAccessExpression;

        public ISyExpression Target { get; set; }
        public string MemberName { get; set; }
    }

    // Target[...Indexes]
    public class SyIndexAccessExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.IndexAccessExpression;

        public ISyExpression Target { get; set; }
        public List<ISyExpression> Indexes { get; set; }
    }

    // Target(...Arguments)
    public class SyCallFuncExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.CallFuncExpression;

        public ISyExpression Target { get; set; }
        public List<ISyExpression> Arguments { get; set; }
    }

    public class SyNotOperator : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.NotOperator;

        public ISyExpression Target { get; set; }
    }

    public class SySignExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.SignExpression;

        public SignKind SignKind { get; set; }
        public ISyExpression Target { get; set; }
    }

    public class SyRelationalOperator : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.RelationalOperator;

        public RelationalOperatorKind RelationalOperatorKind { get; set; }
        public ISyExpression Left { get; set; }
        public ISyExpression Right { get; set; }
    }

    public class SyLogicOperator : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.LogicOperator;

        public LogicOperatorKind LogicOperatorKind { get; set; }
        public ISyExpression Left { get; set; }
        public ISyExpression Right { get; set; }
    }

    public class SyMathOperator : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.MathOperator;

        public MathOperatorKind MathOperatorKind { get; set; }
        public ISyExpression Left { get; set; }
        public ISyExpression Right { get; set; }
    }
}
