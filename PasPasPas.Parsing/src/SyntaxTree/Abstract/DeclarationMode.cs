#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     declaration mode
    /// </summary>
    public enum DeclarationMode {

        /// <summary>
        ///     undefined mode
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     constants
        /// </summary>
        Const = 1,

        /// <summary>
        ///     resource strings
        /// </summary>
        ResourceString = 2,

        /// <summary>
        ///     type section
        /// </summary>
        Types = 3,

        /// <summary>
        ///     var declaration
        /// </summary>
        Var = 4,

        /// <summary>
        ///     thread variables
        /// </summary>
        ThreadVar = 5,

        /// <summary>
        ///     binary exported functions
        /// </summary>
        Exports = 6,

        /// <summary>
        ///     label declaration
        /// </summary>
        Label = 7
    }
}