namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     real type definition
    /// </summary>
    public interface IRealType : IFixedSizeType, IMangledNameTypeSymbol {

        /// <summary>
        ///     real type kind
        /// </summary>
        RealTypeKind Kind { get; }

    }
}
