using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.CodeGen {

    /// <summary>
    ///     interface for constant encoding / decoding
    /// </summary>
    public interface IConstantEncoder {

        /// <summary>
        ///     encode a constant
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ImmutableArray<byte> Encode(IValue value);

        /// <summary>
        ///     decode a constant
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IValue Decode(ImmutableArray<byte> value);

    }
}