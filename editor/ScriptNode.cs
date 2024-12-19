using System.Collections.Generic;

namespace editor
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
        Declare,  // var A = B;              : (string name, Expression expr)
        Assign,   // A = B;                  : (AssignKind kind, Value target, Expression expr)
        Inc,      // A++;                    : (Value target)
        Dec,      // A--;                    : (Value target)
        If,       // if A { B } else { C }   : (Expression expr, List<Instruction> then, List<Instruction> else)
    }

    internal enum AssignKind
    {
        Basic,  // A = B
        Add,    // A += B
        Sub,    // A -= B
        Mul,    // A *= B
        Div,    // A /= B
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
        Value,     // A      : (Value value)
        Equal,     // A == B : (Value left, Value right)
        NotEqual,  // A != B : (Value left, Value right)
        Add,       // A + B  : (Value left, Value right)
        Sub,       // A - B  : (Value left, Value right)
        Mul,       // A * B  : (Value left, Value right)
        Div,       // A / B  : (Value left, Value right)
    }

    internal class Value
    {
        public ValueKind Kind { get; set; }

        public List<object> Children { get; set; }

        public Value(ValueKind kind)
        {
            Kind = kind;
            Children = new List<object>();
        }

        public Value(ValueKind kind, List<object> children)
        {
            Kind = kind;
            Children = children;
        }
    }

    internal enum ValueKind
    {
        Immediate, // A       : (object value)
        Variable,  // A       : (string name)
        Index,     // A[...B] : (Value target, List<Value> indexes)
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
