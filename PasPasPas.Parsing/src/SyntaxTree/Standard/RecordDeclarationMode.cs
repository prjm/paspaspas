#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     current record declaration ode
    /// </summary>
    public enum RecordDeclarationMode {

        /// <summary>
        ///  undefined record declaration mode
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     simple record fields
        /// </summary>
        Fields = 1,

        /// <summary>
        ///     other declaration mode
        /// </summary>
        Other = 2,

        /// <summary>
        ///     static fields
        /// </summary>
        ClassFields = 3,
    }
}
