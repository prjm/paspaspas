namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     type target
    /// </summary>
    public interface ITypeTarget {

        /// <summary>
        ///     value of this type specification target
        /// </summary>
        ITypeSpecification TypeValue { get; set; }

    }
}
