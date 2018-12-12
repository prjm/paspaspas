namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     integer value
    /// </summary>
    public interface IIntegerValue : INumericalValue, IOrdinalValue {

        /// <summary>
        ///     signed value
        /// </summary>
        long SignedValue { get; }

        /// <summary>
        ///     unsigned value
        /// </summary>
        ulong UnsignedValue { get; }

        /// <summary>
        ///     invert all bits of this value (unary complement)
        /// </summary>
        ITypeReference InvertBits();

    }
}
