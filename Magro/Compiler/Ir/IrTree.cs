using System.Collections.Generic;

namespace Magro.Compiler
{
    internal class IrModuleDeclaration
    {
        public string Name { get; set; }
        public List<IrStatement> Statements { get; set; }
    }

    // -------------------------------------------------------------------------------------------
    // statement
    // -------------------------------------------------------------------------------------------

    internal interface IrStatement
    {
        IrStatementKind StatementKind { get; }
    }

    internal enum IrStatementKind
    {
        VariableDeclaration,
        //FunctionDeclaration,
        AssignStatement,
        //IncrementStatement,
        //DecrementStatement,
        //IfStatement,
        //WhileStatement,
        //ForStatement,
        //BreakStatement,
        //ContinueStatement,
        //ReturnStatement,
        //Block,
        //ExpressionStatement,
    }

    internal class IrVariableDeclaration : IrStatement
    {
        public IrStatementKind StatementKind { get; } = IrStatementKind.VariableDeclaration;
        public string Name { get; set; }
        public IrExpression Initializer { get; set; }
    }

    internal class IrAssignStatement : IrStatement
    {
        public IrStatementKind StatementKind { get; } = IrStatementKind.AssignStatement;
        public IrExpression Target { get; set; }
        public IrExpression Content { get; set; }
    }

    // -------------------------------------------------------------------------------------------
    // expression
    // -------------------------------------------------------------------------------------------

    internal interface IrExpression
    {
        IrExpressionKind ExpressionKind { get; }
    }

    internal enum IrExpressionKind
    {
        ValueExpression,
        ReferenceExpression,
        CallFuncExpression,
        NotOperator,
        SignExpression,
        RelationalOperator,
        LogicOperator,
        MathOperator,
        GroupingExpression,
    }

    // Value
    internal class IrValueExpression : IrExpression
    {
        public IrExpressionKind ExpressionKind { get; } = IrExpressionKind.ValueExpression;
        public IrValueKind ValueKind { get; set; }
        public object Value { get; set; }
    }

    internal enum IrValueKind
    {
        Number,
        String,
        Boolean,
        Null = 16,
    }

    // Name
    internal class IrReferenceExpression : IrExpression
    {
        public IrExpressionKind ExpressionKind { get; } = IrExpressionKind.ReferenceExpression;
        public string Name { get; set; }
    }

    internal class IrCallFuncExpression : IrExpression
    {
        public IrExpressionKind ExpressionKind { get; } = IrExpressionKind.CallFuncExpression;
        public IrExpression Target { get; set; }
        public List<IrExpression> Arguments { get; set; }
    }

    internal class IrNotOperator : IrExpression
    {
        public IrExpressionKind ExpressionKind { get; } = IrExpressionKind.NotOperator;
        public IrExpression Target { get; set; }
    }

    internal class IrSignExpression : IrExpression
    {
        public IrExpressionKind ExpressionKind { get; } = IrExpressionKind.SignExpression;
        public IrSignKind SignKind { get; set; }
        public IrExpression Target { get; set; }
    }

    internal enum IrSignKind
    {
        Positive,
        Negative,
    }

    internal class IrRelationalOperator : IrExpression
    {
        public IrExpressionKind ExpressionKind { get; } = IrExpressionKind.RelationalOperator;
        public IrRelationalOperatorKind RelationalOperatorKind { get; set; }
        public IrExpression Left { get; set; }
        public IrExpression Right { get; set; }
    }

    internal enum IrRelationalOperatorKind
    {
        Equal,      // ==
        NotEqual,   // !=
        Gt,         // >
        Lt,         // <
        GtEq,       // >=
        LtEq,       // <=
    }

    internal class IrLogicOperator : IrExpression
    {
        public IrExpressionKind ExpressionKind { get; } = IrExpressionKind.LogicOperator;
        public IrLogicOperatorKind LogicOperatorKind { get; set; }
        public IrExpression Left { get; set; }
        public IrExpression Right { get; set; }
    }

    internal enum IrLogicOperatorKind
    {
        And,    // &&
        Or,     // ||
    }

    internal class IrMathOperator : IrExpression
    {
        public IrExpressionKind ExpressionKind { get; } = IrExpressionKind.MathOperator;
        public IrMathOperatorKind MathOperatorKind { get; set; }
        public IrExpression Left { get; set; }
        public IrExpression Right { get; set; }
    }

    internal enum IrMathOperatorKind
    {
        Add,    // +
        Sub,    // -
        Mul,    // *
        Div,    // /
        Rem,    // %
    }

    internal class IrGroupingExpression : IrExpression
    {
        public IrExpressionKind ExpressionKind { get; } = IrExpressionKind.GroupingExpression;
        public IrExpression Expression { get; set; }
    }
}
