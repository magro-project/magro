using System.Collections.Generic;

namespace Magro.Syake.Syntax
{
    internal interface ISyStatement
    {
        StatementKind StatementKind { get; }
    }

    internal interface ISyDeclaration
    {
        DeclarationKind DeclarationKind { get; }

        string Name { get; set; }
    }

    internal interface ISyExpression
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
        WhileStatement,
        ForStatement,
        BreakStatement,
        ContinueStatement,
        ReturnStatement,
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
        MemberAccessExpression,
        IndexAccessExpression,
        CallFuncExpression,
        NotOperator,
        SignExpression,
        RelationalOperator,
        LogicOperator,
        MathOperator,
    }

    internal enum RelationalOperatorKind
    {
        Equal,      // Left == Right
        NotEqual,   // Left != Right
        Gt,         // Left > Right
        Lt,         // Left < Right
        GtEq,       // Left >= Right
        LtEq,       // Left <= Right
    }

    internal enum LogicOperatorKind
    {
        And,    // Left && Right
        Or,     // Left || Right
    }

    internal enum MathOperatorKind
    {
        Add,    // Left + Right
        Sub,    // Left - Right
        Mul,    // Left * Right
        Div,    // Left / Right
        Rem,    // Left % Right
    }

    internal enum TypeKind
    {
        Number,
        String,
        Boolean,
    }

    internal enum ValueKind
    {
        Number,
        String,
        Boolean,
        Null = 16,
    }

    internal enum SignKind
    {
        Positive,
        Negative,
    }

    internal class SyModuleDeclaration : ISyDeclaration
    {
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.ModuleDeclaration;

        // semantics
        public List<ISyDeclaration> Declarations { get; set; } = new List<ISyDeclaration>();

        public string Name { get; set; }
        public List<ISyStatement> Statements { get; set; }
    }

    // var A = B;
    internal class SyVariableDeclaration : ISyStatement, ISyDeclaration
    {
        public StatementKind StatementKind { get; } = StatementKind.VariableDeclaration;
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.VariableDeclaration;

        public string Name { get; set; }
        public TypeKind TypeKind { get; set; }
        public ISyExpression Initializer { get; set; }
    }

    // function A(...B) { C }
    internal class SyFunctionDeclaration : ISyStatement, ISyDeclaration
    {
        public StatementKind StatementKind { get; } = StatementKind.FunctionDeclaration;
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.FunctionDeclaration;

        public string Name { get; set; }
        public TypeKind ReturnTypeKind { get; set; }
        public List<string> Parameters { get; set; }
        public List<TypeKind> ParameterTypeKindList { get; set; }
        public SyBlock FunctionBlock { get; set; }
    }

    // A = B;
    internal class SyAssignStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.AssignStatement;

        public ISyExpression Target { get; set; }
        public ISyExpression Content { get; set; }
    }

    // A++;
    internal class SyIncrementStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.IncrementStatement;

        public ISyExpression Target { get; set; }
    }

    // A--;
    internal class SyDecrementStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.DecrementStatement;

        public ISyExpression Target { get; set; }
    }

    // if (A) { B } else { C }
    internal class SyIfStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.IfStatement;

        public ISyExpression Condition { get; set; }
        public SyBlock ThenBlock { get; set; }
        public SyBlock ElseBlock { get; set; }
    }

    // while (A) { B }
    internal class SyWhileStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.WhileStatement;

        public ISyExpression Condition { get; set; }
        public SyBlock LoopBlock { get; set; }
    }

    // for (var A in B) { C }
    internal class SyForStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ForStatement;

        public string VariableName { get; set; }
        public ISyExpression Iterable { get; set; }
        public SyBlock LoopBlock { get; set; }
    }

    internal class SyBreakStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.BreakStatement;
    }

    internal class SyContinueStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ContinueStatement;
    }

    internal class SyReturnStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ReturnStatement;

        public bool HasValue => Value != null;
        public ISyExpression Value { get; set; }
    }

    // { }
    internal class SyBlock : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.Block;

        // semantics
        public List<ISyDeclaration> Declarations { get; set; } = new List<ISyDeclaration>();

        public List<ISyStatement> Statements { get; set; }
    }

    // A;
    internal class SyExpressionStatement : ISyStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ExpressionStatement;

        public ISyExpression Expression { get; set; }
    }

    // Value
    internal class SyValueExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.ValueExpression;

        public ValueKind ValueKind { get; set; }
        public object Value { get; set; }
    }

    // Name
    internal class SyReferenceExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.ReferenceExpression;

        // semantics
        public ISyDeclaration ResolvedDeclaration { get; set; }

        public string Name { get; set; }
    }

    // Target.MemberName
    internal class SyMemberAccessExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.MemberAccessExpression;

        public ISyExpression Target { get; set; }
        public string MemberName { get; set; }
    }

    // Target[...Indexes]
    internal class SyIndexAccessExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.IndexAccessExpression;

        public ISyExpression Target { get; set; }
        public List<ISyExpression> Indexes { get; set; }
    }

    // Target(...Arguments)
    internal class SyCallFuncExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.CallFuncExpression;

        public ISyExpression Target { get; set; }
        public List<ISyExpression> Arguments { get; set; }
    }

    internal class SyNotOperator : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.NotOperator;

        public ISyExpression Target { get; set; }
    }

    internal class SySignExpression : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.SignExpression;

        public SignKind SignKind { get; set; }
        public ISyExpression Target { get; set; }
    }

    internal class SyRelationalOperator : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.RelationalOperator;

        public RelationalOperatorKind RelationalOperatorKind { get; set; }
        public ISyExpression Left { get; set; }
        public ISyExpression Right { get; set; }
    }

    internal class SyLogicOperator : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.LogicOperator;

        public LogicOperatorKind LogicOperatorKind { get; set; }
        public ISyExpression Left { get; set; }
        public ISyExpression Right { get; set; }
    }

    internal class SyMathOperator : ISyExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.MathOperator;

        public MathOperatorKind MathOperatorKind { get; set; }
        public ISyExpression Left { get; set; }
        public ISyExpression Right { get; set; }
    }
}
