using System.Collections.Generic;

namespace Magro.Compiler
{
    internal class SyModuleDeclaration : SyDeclaration
    {
        public SyDeclarationKind DeclarationKind { get; } = SyDeclarationKind.ModuleDeclaration;
        public string Name { get; set; }
        public List<SyStatement> Statements { get; set; }
        // semantics
        public List<SyDeclaration> Declarations { get; set; } = new List<SyDeclaration>();
    }

    internal interface SyDeclaration
    {
        SyDeclarationKind DeclarationKind { get; }
        string Name { get; set; }
    }

    internal enum SyDeclarationKind
    {
        ModuleDeclaration,
        FunctionDeclaration,
        VariableDeclaration,
    }

    // -------------------------------------------------------------------------------------------
    // statement
    // -------------------------------------------------------------------------------------------

    internal interface SyStatement
    {
        SyStatementKind StatementKind { get; }
    }

    internal enum SyStatementKind
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

    // var A = B;
    internal class SyVariableDeclaration : SyStatement, SyDeclaration
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.VariableDeclaration;
        public SyDeclarationKind DeclarationKind { get; } = SyDeclarationKind.VariableDeclaration;
        public SyTypeKind TypeKind { get; set; }
        public string Name { get; set; }
        public SyExpression Initializer { get; set; }
    }

    // function A(...B) { C }
    internal class SyFunctionDeclaration : SyStatement, SyDeclaration
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.FunctionDeclaration;
        public SyDeclarationKind DeclarationKind { get; } = SyDeclarationKind.FunctionDeclaration;
        public SyTypeKind ReturnTypeKind { get; set; }
        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public List<SyTypeKind> ParameterTypeKindList { get; set; }
        public SyBlock FunctionBlock { get; set; }
    }

    internal enum SyTypeKind
    {
        Number,
        String,
        Boolean,
    }

    // A = B;
    internal class SyAssignStatement : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.AssignStatement;
        public SyExpression Target { get; set; }
        public SyExpression Content { get; set; }
    }

    // A++;
    internal class SyIncrementStatement : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.IncrementStatement;
        public SyExpression Target { get; set; }
    }

    // A--;
    internal class SyDecrementStatement : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.DecrementStatement;
        public SyExpression Target { get; set; }
    }

    // if (A) { B } else { C }
    internal class SyIfStatement : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.IfStatement;
        public SyExpression Condition { get; set; }
        public SyBlock ThenBlock { get; set; }
        public SyBlock ElseBlock { get; set; }
    }

    // while (A) { B }
    internal class SyWhileStatement : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.WhileStatement;
        public SyExpression Condition { get; set; }
        public SyBlock LoopBlock { get; set; }
    }

    // for (var A in B) { C }
    internal class SyForStatement : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.ForStatement;
        public string VariableName { get; set; }
        public SyExpression Iterable { get; set; }
        public SyBlock LoopBlock { get; set; }
    }

    internal class SyBreakStatement : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.BreakStatement;
    }

    internal class SyContinueStatement : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.ContinueStatement;
    }

    internal class SyReturnStatement : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.ReturnStatement;
        public bool HasValue => Value != null;
        public SyExpression Value { get; set; }
    }

    // { }
    internal class SyBlock : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.Block;
        public List<SyStatement> Statements { get; set; }
        // semantics
        public List<SyDeclaration> Declarations { get; set; } = new List<SyDeclaration>();
    }

    // A;
    internal class SyExpressionStatement : SyStatement
    {
        public SyStatementKind StatementKind { get; } = SyStatementKind.ExpressionStatement;
        public SyExpression Expression { get; set; }
    }

    // -------------------------------------------------------------------------------------------
    // expression
    // -------------------------------------------------------------------------------------------

    internal interface SyExpression
    {
        SyExpressionKind ExpressionKind { get; }
    }

    internal enum SyExpressionKind
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
        GroupingExpression,
    }

    // Value
    internal class SyValueExpression : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.ValueExpression;
        public SyValueKind ValueKind { get; set; }
        public object Value { get; set; }
    }

    internal enum SyValueKind
    {
        Number,
        String,
        Boolean,
        Null = 16,
    }

    // Name
    internal class SyReferenceExpression : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.ReferenceExpression;
        public string Name { get; set; }
        // semantics
        public SyDeclaration ResolvedDeclaration { get; set; }
    }

    // Target.MemberName
    internal class SyMemberAccessExpression : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.MemberAccessExpression;
        public SyExpression Target { get; set; }
        public string MemberName { get; set; }
    }

    // Target[...Indexes]
    internal class SyIndexAccessExpression : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.IndexAccessExpression;
        public SyExpression Target { get; set; }
        public List<SyExpression> Indexes { get; set; }
    }

    // Target(...Arguments)
    internal class SyCallFuncExpression : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.CallFuncExpression;
        public SyExpression Target { get; set; }
        public List<SyExpression> Arguments { get; set; }
    }

    internal class SyNotOperator : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.NotOperator;
        public SyExpression Target { get; set; }
    }

    internal class SySignExpression : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.SignExpression;
        public SySignKind SignKind { get; set; }
        public SyExpression Target { get; set; }
    }

    internal enum SySignKind
    {
        Positive, // +
        Negative, // -
    }

    internal class SyRelationalOperator : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.RelationalOperator;
        public SyRelationalOperatorKind RelationalOperatorKind { get; set; }
        public SyExpression Left { get; set; }
        public SyExpression Right { get; set; }
    }

    internal enum SyRelationalOperatorKind
    {
        Equal,      // ==
        NotEqual,   // !=
        Gt,         // >
        Lt,         // <
        GtEq,       // >=
        LtEq,       // <=
    }

    internal class SyLogicOperator : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.LogicOperator;
        public SyLogicOperatorKind LogicOperatorKind { get; set; }
        public SyExpression Left { get; set; }
        public SyExpression Right { get; set; }
    }

    internal enum SyLogicOperatorKind
    {
        And,    // &&
        Or,     // ||
    }

    internal class SyMathOperator : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.MathOperator;
        public SyMathOperatorKind MathOperatorKind { get; set; }
        public SyExpression Left { get; set; }
        public SyExpression Right { get; set; }
    }

    internal enum SyMathOperatorKind
    {
        Add,    // +
        Sub,    // -
        Mul,    // *
        Div,    // /
        Rem,    // %
    }

    internal class SyGroupingExpression : SyExpression
    {
        public SyExpressionKind ExpressionKind { get; } = SyExpressionKind.GroupingExpression;
        public SyExpression Expression { get; set; }
    }
}
