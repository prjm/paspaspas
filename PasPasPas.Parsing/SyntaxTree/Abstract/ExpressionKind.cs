namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     undefined expression kind
    /// </summary>
    public enum ExpressionKind {

        /// <summary>
        ///     undefined
        /// </summary>      
        Undefined = 0,

        /// <summary>
        ///     <code>&lt;</code>
        /// </summary>
        LessThen = 1,

        /// <summary>
        ///     <code>&lt;=</code>
        /// </summary>
        LessThenEquals = 2,

        /// <summary>
        ///     <code>&gt;</code>
        /// </summary>
        GreaterThen = 3,

        /// <summary>
        ///     <code>&gt;=</code>
        /// </summary>
        GreaterThenEquals = 4,

        /// <summary>
        ///     <code>&lt;&gt;</code>
        /// </summary>
        NotEquals = 5,

        /// <summary>
        ///     <code>As</code>
        /// </summary>
        As = 6,

        /// <summary>
        ///     <code>In</code>
        /// </summary>
        In = 7,

        /// <summary>
        ///     <code>=</code>
        /// </summary>
        EqualsSign = 8,

        /// <summary>
        ///     xor
        /// </summary>
        Xor = 9,

        /// <summary>
        ///     or
        /// </summary>
        Or = 10,

        /// <summary>
        ///     minus
        /// </summary>
        Minus = 11,

        /// <summary>
        ///     plus
        /// </summary>
        Plus = 12,

        /// <summary>
        ///     shr
        /// </summary>
        Shr = 13,

        /// <summary>
        ///     shlr
        /// </summary>
        Shl = 14,

        /// <summary>
        ///     and
        /// </summary>
        And = 15,

        /// <summary>
        ///     mod
        /// </summary>
        Mod = 16,

        /// <summary>
        ///     slash
        /// </summary>
        Slash = 17,

        /// <summary>
        ///     times
        /// </summary>
        Times = 18,

        /// <summary>
        ///     div
        /// </summary>
        Div = 19,

        /// <summary>
        ///     not
        /// </summary>
        Not = 20,

        /// <summary>
        ///     address of
        /// </summary>
        AddressOf = 21,

        /// <summary>
        ///     unary minus
        /// </summary>
        UnaryMinus = 22,

        /// <summary>
        ///     unary plus
        /// </summary>
        UnaryPlus = 23,
    }
}