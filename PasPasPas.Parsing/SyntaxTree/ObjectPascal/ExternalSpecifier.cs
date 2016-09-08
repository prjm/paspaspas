namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     external specifier
    /// </summary>
    public class ExternalSpecifier : SyntaxPartBase {

        /// <summary>
        ///     external expression
        /// </summary>
        public ConstantExpression Expression { get; set; }

        /// <summary>
        ///     external specifier kind
        /// </summary>
        public int Kind { get; set; }


    }
}