using System.Collections.Generic;

namespace Magro.Ikura
{
    internal class IkModuleDeclaration
    {
        public string Name { get; set; }
        public List<IkVariableDeclaration> Variables { get; set; }
        public List<IIkStatement> Statements { get; set; }
    }

    internal class IkVariableDeclaration
    {
        public string Name { get; set; }
        public IIkType Type { get; set; }
    }

    internal class IkFunctionDeclaration
    {
        public string Name { get; set; }
        public IIkType ReturnType { get; set; }
        public List<IIkType> ParameterTypeList { get; set; }
    }

    // statement

    internal interface IIkStatement
    {
        IkStatementKind StatementKind { get; }
    }

    internal enum IkStatementKind
    {
        Assign,
        CallFunction,
    }

    internal class IkAssign : IIkStatement
    {
        public IkStatementKind StatementKind { get; } = IkStatementKind.Assign;

        public IkVariableDeclaration Target { get; set; }
        public IIkValue Value { get; set; }
    }

    internal class IkCallFunction : IIkStatement
    {
        public IkStatementKind StatementKind { get; } = IkStatementKind.CallFunction;

        public IkVariableDeclaration ResultTarget { get; set; }
        public List<IIkValue> ArgumentList { get; set; }
    }

    // value

    internal interface IIkValue
    {
        IkValueKind ValueKind { get; }
    }

    internal enum IkValueKind
    {
        Literal,
        List,
        Table,
    }

    internal enum IkItemKind
    {
        String,
        Number,
        Boolean,
        Null = 16,
    }

    internal class IkLiteralValue : IIkValue
    {
        public IkValueKind ValueKind { get; } = IkValueKind.Literal;

        public IkItemKind ItemKind { get; set; }
        public object Source { get; set; }
    }

    internal class IkListValue : IIkValue
    {
        public IkValueKind ValueKind { get; } = IkValueKind.List;

        public IkItemKind ItemKind { get; set; }
        public List<object> Source { get; set; }
    }

    internal class IkTableValue : IIkValue
    {
        public IkValueKind ValueKind { get; } = IkValueKind.Table;

        public IkItemKind ItemKind { get; set; }
        public List<List<object>> Source { get; set; }
    }

    // types

    internal interface IIkType
    {
        IkTypeKind TypeKind { get; }
    }

    internal enum IkTypeKind
    {
        Literal,
        List,
        Table,
    }

    internal class IkLiteralType : IIkType
    {
        public IkTypeKind TypeKind { get; } = IkTypeKind.Literal;

        public IkLiteralTypeKind LiteralTypeKind { get; set; }
    }

    internal enum IkLiteralTypeKind
    {
        String,
        Number,
        Boolean,
    }

    internal class IkListType : IIkType
    {
        public IkTypeKind TypeKind { get; } = IkTypeKind.List;

        public IIkType ItemType { get; set; }
    }

    internal class IkTableType : IIkType
    {
        public IkTypeKind TypeKind { get; } = IkTypeKind.Table;

        public IIkType ItemType { get; set; }
    }
}
