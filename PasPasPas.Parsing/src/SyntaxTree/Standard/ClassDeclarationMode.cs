#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     current class declaration mode
    /// </summary>
    public enum ClassDeclarationMode {


        /// <summary>
        ///     undefined mode
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     declaring fields
        /// </summary>
        Fields = 1,

        /// <summary>
        ///     declaring class fields
        /// </summary>
        ClassFields = 2,

        /// <summary>
        ///     declaring other things (methods, properties, constants, etc.)
        /// </summary>
        Other = 3,

    }
}
