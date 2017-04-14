namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     reference part kind
    /// </summary>
    public enum SymbolReferencePartKind {


        /// <summary>
        ///     unknown part
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     dereference
        /// </summary>
        Dereference = 1,

        /// <summary>
        ///     subsequent item
        /// </summary>
        SubItem = 2,

        /// <summary>
        ///     array index
        /// </summary>
        ArrayIndex = 3,

        /// <summary>
        ///     parameters for a function call
        /// </summary>
        CallParameters = 4,
    }
}
