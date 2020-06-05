#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     symbol kind
    /// </summary>
    public enum AsmPrefixSymbolKind {

        /// <summary>
        ///     unknown symbol
        /// </summary>
        Unknown,

        /// <summary>
        ///     push operation
        /// </summary>
        PushEnvOperation,

        /// <summary>
        ///     params operation
        /// </summary>
        ParamsOperation,

        /// <summary>
        ///     no frame statement
        /// </summary>
        NoFrame,

        /// <summary>
        ///     push env operation
        /// </summary>
        SaveEnvOperation
    };
}
