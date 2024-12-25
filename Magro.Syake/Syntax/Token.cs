namespace Magro.Syake.Syntax
{
    internal class Token
    {
        public TokenKind TokenKind { get; set; }
        public Location BeginLocation { get; set; }
        public Location EndLocation { get; set; }
        public string Content { get; set; }

        public Token(TokenKind tokenKind, Location beginLocation, Location endLocation)
        {
            TokenKind = tokenKind;
            BeginLocation = beginLocation;
            EndLocation = endLocation;
        }

        public override string ToString()
        {
            switch (TokenKind)
            {
                case TokenKind.EOF: return "EOF";
                case TokenKind.OpenBracket: return "'['";
                case TokenKind.CloseBracket: return "']'";
                case TokenKind.OpenBrace: return "'{'";
                case TokenKind.CloseBrace: return "'}'";
                case TokenKind.OpenParen: return "'('";
                case TokenKind.CloseParen: return "')'";
                case TokenKind.Comma: return "','";
                case TokenKind.Dot: return "'.'";
                case TokenKind.SemiCollon: return "';'";
                case TokenKind.Equal: return "'='";
                case TokenKind.Equal2: return "'=='";
                case TokenKind.NotEqual: return "'!='";
                case TokenKind.Gt: return "'>'";
                case TokenKind.GtEq: return "'>='";
                case TokenKind.Lt: return "'<'";
                case TokenKind.LtEq: return "'<='";
                case TokenKind.Not: return "'!'";
                case TokenKind.Plus: return "'+'";
                case TokenKind.Plus2: return "'++'";
                case TokenKind.Minus: return "'-'";
                case TokenKind.Minus2: return "'--'";
                case TokenKind.Astarisk: return "'*'";
                case TokenKind.Slash: return "'/'";
                case TokenKind.Percent: return "'%'";
                case TokenKind.And2: return "'&&'";
                case TokenKind.Or2: return "'||'";
            }

            if (TokenKind == TokenKind.Word)
            {
                return "'" + Content + "'";
            }

            if (TokenKind == TokenKind.Number)
            {
                return "'" + Content + "'";
            }

            throw new System.Exception("Invalid token");
        }
    }

    internal enum TokenKind
    {
        // SpacesTrivia,
        // NewLineTrivia,
        // CommentLineTrivia,
        // CommentRangeTrivia,

        EOF,

        Word,
        Number,
        String,

        /// <summary>
        /// "["
        /// </summary>
        OpenBracket,

        /// <summary>
        /// "]"
        /// </summary>
        CloseBracket,

        /// <summary>
        /// "{"
        /// </summary>
        OpenBrace,

        /// <summary>
        /// "}"
        /// </summary>
        CloseBrace,

        /// <summary>
        /// "("
        /// </summary>
        OpenParen,

        /// <summary>
        /// ")"
        /// </summary>
        CloseParen,

        /// <summary>
        /// ","
        /// </summary>
        Comma,

        /// <summary>
        /// "."
        /// </summary>
        Dot,

        /// <summary>
        /// ";"
        /// </summary>
        SemiCollon,

        /// <summary>
        /// "="
        /// </summary>
        Equal,

        /// <summary>
        /// "=="
        /// </summary>
        Equal2,

        /// <summary>
        /// "!="
        /// </summary>
        NotEqual,

        /// <summary>
        /// ">"
        /// </summary>
        Gt,

        /// <summary>
        /// ">="
        /// </summary>
        GtEq,

        /// <summary>
        /// "<"
        /// </summary>
        Lt,

        /// <summary>
        /// "<="
        /// </summary>
        LtEq,

        /// <summary>
        /// "!"
        /// </summary>
        Not,

        /// <summary>
        /// "+"
        /// </summary>
        Plus,

        /// <summary>
        /// "++"
        /// </summary>
        Plus2,

        /// <summary>
        /// "-"
        /// </summary>
        Minus,

        /// <summary>
        /// "--"
        /// </summary>
        Minus2,

        /// <summary>
        /// "*"
        /// </summary>
        Astarisk,

        /// <summary>
        /// "/"
        /// </summary>
        Slash,

        /// <summary>
        /// "%"
        /// </summary>
        Percent,

        /// <summary>
        /// &&
        /// </summary>
        And2,

        /// <summary>
        /// ||
        /// </summary>
        Or2,
    }
}
