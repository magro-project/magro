using System.Collections.Generic;

namespace Magro.Ikura
{
    internal class IkModuleDeclaration
    {
        public string Name { get; set; }
        public List<IkVariableDeclaration> Variables { get; set; }
        public List<IkStatement> Statements { get; set; }
    }

    internal class IkVariableDeclaration
    {
        public string Name { get; set; }
        public IkType Type { get; set; }
    }

    internal class IkFunctionDeclaration
    {
        public string Name { get; set; }
        public IkType ReturnType { get; set; }
        public List<IkType> ParameterTypeList { get; set; }
    }

    // -------------------------------------------------------------------------------------------
    // statement
    // -------------------------------------------------------------------------------------------

    internal interface IkStatement
    {
        IkStatementKind StatementKind { get; }
    }

    internal enum IkStatementKind
    {
        Assign,
        CallFunction,
    }

    internal class IkAssign : IkStatement
    {
        public IkStatementKind StatementKind { get; } = IkStatementKind.Assign;
        public IkVariableDeclaration Target { get; set; }
        public IkValue Value { get; set; }
    }

    internal class IkCallFunction : IkStatement
    {
        public IkStatementKind StatementKind { get; } = IkStatementKind.CallFunction;
        public IkVariableDeclaration ResultTarget { get; set; }
        public List<IkValue> ArgumentList { get; set; }
    }

    // -------------------------------------------------------------------------------------------
    // value
    // -------------------------------------------------------------------------------------------

    internal interface IkValue
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

    internal class IkLiteralValue : IkValue
    {
        public IkValueKind ValueKind { get; } = IkValueKind.Literal;
        public IkItemKind ItemKind { get; set; }
        public object Source { get; set; }
    }

    internal class IkListValue : IkValue
    {
        public IkValueKind ValueKind { get; } = IkValueKind.List;
        public IkItemKind ItemKind { get; set; }
        public List<object> Source { get; set; }
    }

    internal class IkTableValue : IkValue
    {
        public IkValueKind ValueKind { get; } = IkValueKind.Table;
        public IkItemKind ItemKind { get; set; }
        public List<List<object>> Source { get; set; }
    }

    // -------------------------------------------------------------------------------------------
    // type
    // -------------------------------------------------------------------------------------------

    internal interface IkType
    {
        IkTypeKind TypeKind { get; }
    }

    internal enum IkTypeKind
    {
        Literal,
        List,
        Table,
    }

    internal class IkLiteralType : IkType
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

    internal class IkListType : IkType
    {
        public IkTypeKind TypeKind { get; } = IkTypeKind.List;
        public IkType ItemType { get; set; }
    }

    internal class IkTableType : IkType
    {
        public IkTypeKind TypeKind { get; } = IkTypeKind.Table;
        public IkType ItemType { get; set; }
    }
}
