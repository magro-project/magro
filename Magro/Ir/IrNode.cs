using System.Collections.Generic;

namespace Magro.Ir
{
    public interface IIrStatement
    {
        StatementKind StatementKind { get; }
    }

    public interface IIrDeclaration
    {
        DeclarationKind DeclarationKind { get; }

        string Name { get; set; }
    }

    public interface IIrExpression
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

    public class IrModuleDeclaration : IIrDeclaration
    {
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.ModuleDeclaration;

        // semantics
        public List<IIrDeclaration> Declarations { get; set; } = new List<IIrDeclaration>();

        public string Name { get; set; }
        public List<IIrStatement> Statements { get; set; }
    }

    // var A = B;
    public class IrVariableDeclaration : IIrStatement, IIrDeclaration
    {
        public StatementKind StatementKind { get; } = StatementKind.VariableDeclaration;
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.VariableDeclaration;

        public string Name { get; set; }
        public IIrExpression Initializer { get; set; }
    }

    // function A(...B) { C }
    public class IrFunctionDeclaration : IIrStatement, IIrDeclaration
    {
        public StatementKind StatementKind { get; } = StatementKind.FunctionDeclaration;
        public DeclarationKind DeclarationKind { get; } = DeclarationKind.FunctionDeclaration;

        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public IrBlock FunctionBlock { get; set; }
    }

    // A = B;
    public class IrAssignStatement : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.AssignStatement;

        public IIrExpression Target { get; set; }
        public IIrExpression Content { get; set; }
    }

    // A++;
    public class IrIncrementStatement : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.IncrementStatement;

        public IIrExpression Target { get; set; }
    }

    // A--;
    public class IrDecrementStatement : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.DecrementStatement;

        public IIrExpression Target { get; set; }
    }

    // if (A) { B } else { C }
    public class IrIfStatement : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.IfStatement;

        public IIrExpression Condition { get; set; }
        public IrBlock ThenBlock { get; set; }
        public IrBlock ElseBlock { get; set; }
    }

    // while (A) { B }
    public class IrWhileStatement : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.WhileStatement;

        public IIrExpression Condition { get; set; }
        public IrBlock LoopBlock { get; set; }
    }

    // for (var A in B) { C }
    public class IrForStatement : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ForStatement;

        public string VariableName { get; set; }
        public IIrExpression Iterable { get; set; }
        public IrBlock LoopBlock { get; set; }
    }

    public class IrBreakStatement : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.BreakStatement;
    }

    public class IrContinueStatement : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ContinueStatement;
    }

    public class IrReturnStatement : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ReturnStatement;

        public bool HasValue => Value != null;
        public IIrExpression Value { get; set; }
    }

    // { }
    public class IrBlock : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.Block;

        // semantics
        public List<IIrDeclaration> Declarations { get; set; } = new List<IIrDeclaration>();

        public List<IIrStatement> Statements { get; set; }
    }

    // A;
    public class IrExpressionStatement : IIrStatement
    {
        public StatementKind StatementKind { get; } = StatementKind.ExpressionStatement;

        public IIrExpression Expression { get; set; }
    }

    // Value
    public class IrValueExpression : IIrExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.ValueExpression;

        public ValueKind ValueKind { get; set; }
        public object Value { get; set; }
    }

    // Name
    public class IrReferenceExpression : IIrExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.ReferenceExpression;

        // semantics
        public IIrDeclaration ResolvedDeclaration { get; set; }

        public string Name { get; set; }
    }

    // Target.MemberName
    public class IrMemberAccessExpression : IIrExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.MemberAccessExpression;

        public IIrExpression Target { get; set; }
        public string MemberName { get; set; }
    }

    // Target[...Indexes]
    public class IrIndexAccessExpression : IIrExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.IndexAccessExpression;

        public IIrExpression Target { get; set; }
        public List<IIrExpression> Indexes { get; set; }
    }

    // Target(...Arguments)
    public class IrCallFuncExpression : IIrExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.CallFuncExpression;

        public IIrExpression Target { get; set; }
        public List<IIrExpression> Arguments { get; set; }
    }

    public class IrNotOperator : IIrExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.NotOperator;

        public IIrExpression Target { get; set; }
    }

    public class IrSignExpression : IIrExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.SignExpression;

        public SignKind SignKind { get; set; }
        public IIrExpression Target { get; set; }
    }

    public class IrRelationalOperator : IIrExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.RelationalOperator;

        public RelationalOperatorKind RelationalOperatorKind { get; set; }
        public IIrExpression Left { get; set; }
        public IIrExpression Right { get; set; }
    }

    public class IrLogicOperator : IIrExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.LogicOperator;

        public LogicOperatorKind LogicOperatorKind { get; set; }
        public IIrExpression Left { get; set; }
        public IIrExpression Right { get; set; }
    }

    public class IrMathOperator : IIrExpression
    {
        public ExpressionKind ExpressionKind { get; } = ExpressionKind.MathOperator;

        public MathOperatorKind MathOperatorKind { get; set; }
        public IIrExpression Left { get; set; }
        public IIrExpression Right { get; set; }
    }
}
