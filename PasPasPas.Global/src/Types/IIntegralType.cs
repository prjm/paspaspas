namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for integral types
    /// </summary>
    public interface IIntegralType : IOrdinalType {

        /// <summary>
        ///     <c>true</c> if this type is signed
        /// </summary>
        bool IsSigned { get; }
    }
}
