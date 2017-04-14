namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     generic types
    /// </summary>
    public class GenericTypes : SymbolTableBase<GenericType>, ITypeTarget {

        /// <summary>
        ///     reference to type
        /// </summary>
        public bool TypeReference { get; set; }

        /// <summary>
        ///     type reference
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

    }
}
