namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     undefined expression kind
    /// </summary>
    public enum ComparisonExpressionKind {

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
    }
}