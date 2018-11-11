using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for ordinal types
    /// </summary>
    public interface IOrdinalType : IFixedSizeType {

        /// <summary>
        ///     highest element
        /// </summary>
        ITypeReference HighestElement { get; }

        /// <summary>
        ///     lowest element
        /// </summary>
        ITypeReference LowestElement { get; }

    }
}