namespace PasPasPas.Parsing.SyntaxTree.Standard {

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
        public Identifier Subitem { get; set; }

        /// <summary>
        ///     generic type of the subitem
        /// </summary>
        public GenericSuffix SubitemGenericType { get; set; }

        /// <summary>
        ///     <true>if a parameter list</true>
        /// </summary>
        public bool ParameterList { get; set; }
    }
}