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
        ImmutableArray<byte> Encode(IOldTypeReference value);

        /// <summary>
        ///     decode a constant
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IOldTypeReference Decode(ImmutableArray<byte> value);

    }
}