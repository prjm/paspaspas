#nullable disable
using System.Numerics;
using PasPasPas.Globals.Types;

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
        ///     big int value
        /// </summary>
        BigInteger AsBigInteger { get; }

        /// <summary>
        ///     invert all bits of this value (unary complement)
        /// </summary>
        IValue InvertBits();

        /// <summary>
        ///     integral type
        /// </summary>
        IIntegralType IntegralType { get; }

    }
}
