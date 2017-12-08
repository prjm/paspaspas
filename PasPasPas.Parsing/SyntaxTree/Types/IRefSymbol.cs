namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     referenceable symbol
    /// </summary>
    public interface IRefSymbol {

        /// <summary>
        ///     get the type of this symbol
        /// </summary>
        int TypeId { get; }
    }
}
