namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     reference to a symbol
    /// </summary>
    public class SymbolReference : AbstractSyntaxPart, IExpression, ILabelTarget, ITypeTarget {

        /// <summary>
        ///     identifier name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     <c>true</c> if a pointer was used
        /// </summary>
        public bool PointerTo { get; set; }

        /// <summary>
        ///     referencing label
        /// </summary>
        public SymbolName LabelName { get; set; }

        /// <summary>
        ///     reference to inherited symbol
        /// </summary>
        public bool Inherited { get; set; }

        /// <summary>
        ///     type name for designators
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

    }
}
