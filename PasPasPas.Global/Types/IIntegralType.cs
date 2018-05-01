namespace PasPasPas.Global.Types {

    /// <summary>
    ///     interface for integral types
    /// </summary>
    public interface IIntegralType : IOrdinalType {

        /// <summary>
        ///     <c>true</c> if this type is signed
        /// </summary>
        bool Signed { get; }
    }
}
