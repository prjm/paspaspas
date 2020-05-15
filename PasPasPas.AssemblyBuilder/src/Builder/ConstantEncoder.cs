using System.Collections.Immutable;
using System.IO;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.AssemblyBuilder.Builder {

    /// <summary>
    ///     constant encoder
    /// </summary>
    public class ConstantEncoder : IConstantEncoder {

        /// <summary>
        ///     create a new constant encoder
        /// </summary>
        /// <param name="environmnet"></param>
        public ConstantEncoder(ITypedEnvironment environmnet)
            => Environment = environmnet;

        /// <summary>
        ///     runtime
        /// </summary>
        public ITypedEnvironment Environment { get; }

        /// <summary>
        ///     decode a runtime constant
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValue Decode(ImmutableArray<byte> value) {
            using (var stream = new ImmutableByteArrayStream(value))
            using (var typeReader = Environment.CreateTypeReader(stream)) {

                return typeReader.ReadConstant();

            }
        }

        /// <summary>
        ///     encode an integer value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ImmutableArray<byte> Encode(IValue value) {
            using (var stream = new MemoryStream())
            using (var writer = Environment.CreateTypeWriter(stream)) {

                writer.WriteConstant(value);
                return ImmutableArray.Create(stream.ToArray());
            }

        }

    }
}
