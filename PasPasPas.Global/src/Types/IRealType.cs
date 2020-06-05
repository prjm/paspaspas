#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     real type definition
    /// </summary>
    public interface IRealType : IFixedSizeType {

        /// <summary>
        ///     real type kind
        /// </summary>
        RealTypeKind Kind { get; }

    }
}
