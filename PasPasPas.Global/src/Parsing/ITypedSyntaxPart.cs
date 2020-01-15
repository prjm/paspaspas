using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Parsing {

    /// <summary>
    ///     interface for typed syntax nodes
    /// </summary>
    public interface ITypedSyntaxPart : ISyntaxPart {

        /// <summary>
        ///     type information
        /// </summary>
        IOldTypeReference TypeInfo { get; set; }

    }
}
