namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for integral types
    /// </summary>
    public interface IIntegralType : IOrdinalType {

        /// <summary>
        ///     type kind
        /// </summary>
        IntegralTypeKind Kind { get; }

    }
}
