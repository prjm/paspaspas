namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     designator item
    /// </summary>
    public class DesignatorItem : SyntaxPartBase {

        /// <summary>
        ///     dereference
        /// </summary>
        public bool Dereference { get; set; }

        /// <summary>
        ///     index expression
        /// </summary>
        public ExpressionList IndexExpression { get; set; }

        /// <summary>
        ///     subitem
        /// </summary>
        public PascalIdentifier Subitem { get; set; }

    }
}