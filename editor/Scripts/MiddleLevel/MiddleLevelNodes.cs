using System.Collections.Generic;

namespace Magro.Scripts.MiddleLevel
{
    internal class Module
    {
        public string Name { get; set; }

        public List<Instruction> Instructions { get; set; }

        public Module(string name)
        {
            Name = name;
            Instructions = new List<Instruction>();
        }

        public Module(string name, List<Instruction> instructions)
        {
            Name = name;
            Instructions = instructions;
        }
    }

    internal class Instruction
    {
        public InstructionKind Kind { get; set; }

        public List<object> Children { get; set; }

        public Instruction(InstructionKind kind)
        {
            Kind = kind;
            Children = new List<object>();
        }

        public Instruction(InstructionKind kind, List<object> children)
        {
            Kind = kind;
            Children = children;
        }
    }

    internal enum InstructionKind
    {
        Declare,  // var A = B;              : (string name, Expression? expr)
        Assign,   // A = B;                  : (AssignKind kind, Expression target, Expression expr)
        Inc,      // A++;                    : (Expression target)
        Dec,      // A--;                    : (Expression target)
        If,       // if A { B } else { C }   : (Expression expr, List<Instruction> then, List<Instruction> else)
        Call,     // A(...B)                 : (Expression target, List<Expression> args)
    }

    internal enum AssignKind
    {
        Basic, // A = B
        Add,   // A += B
        Sub,   // A -= B
        Mul,   // A *= B
        Div,   // A /= B
        Rem,   // A %= B
    }



    internal class Expression
    {
        public ExpressionKind Kind { get; set; }

        public List<object> Children { get; set; }

        public Expression(ExpressionKind kind)
        {
            Kind = kind;
            Children = new List<object>();
        }

        public Expression(ExpressionKind kind, List<object> children)
        {
            Kind = kind;
            Children = children;
        }
    }

    internal enum ExpressionKind
    {
        ImmediateValue, // (object value) : A
        VariableRef,    // (string name) : A
        Index,          // (Expression target, List<Expression> indexes) : A[...B]
        Equal,          // (Expression left, Expression right) : A == B
        NotEqual,       // (Expression left, Expression right) : A != B
        Add,            // (Expression left, Expression right) : A + B
        Sub,            // (Expression left, Expression right) : A - B
        Mul,            // (Expression left, Expression right) : A * B
        Div,            // (Expression left, Expression right) : A / B
        Rem,            // (Expression left, Expression right) : A % B
    }



    internal class Function
    {
        public List<Instruction> Instructions { get; set; }

        public Function(string name)
        {
            Instructions = new List<Instruction>();
        }

        public Function(string name, List<Instruction> instructions)
        {
            Instructions = instructions;
        }
    }
}
