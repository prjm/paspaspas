namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for integral types
    /// </summary>
    public interface IIntegralType : IOrdinalType, IMangledNameTypeSymbol {

        /// <summary>
        ///     type kind
        /// </summary>
        IntegralTypeKind Kind { get; }

    }
}
