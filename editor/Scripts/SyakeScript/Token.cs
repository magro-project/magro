namespace Magro.Scripts.SyakeScript
{
    internal class Token
    {
        public TokenKind TokenKind { get; set; }
        public Location BeginLocation { get; set; }
        public Location EndLocation { get; set; }
        public object Content { get; set; }

        public Token(TokenKind tokenKind, Location beginLocation, Location endLocation)
        {
            TokenKind = tokenKind;
            BeginLocation = beginLocation;
            EndLocation = endLocation;
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
    }
}
