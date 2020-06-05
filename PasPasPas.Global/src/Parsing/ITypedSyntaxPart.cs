#nullable disable
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Parsing {

    /// <summary>
    ///     interface for typed syntax nodes
    /// </summary>
    public interface ITypedSyntaxPart : ISyntaxPart {

        /// <summary>
        ///     type information
        /// </summary>
        ITypeSymbol TypeInfo { get; set; }

    }
}
