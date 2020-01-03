using System.Collections.Immutable;

namespace PasPasPas.Globals.CodeGen {

    /// <summary>
    ///     op code id
    /// </summary>
    public readonly struct OpCode {

        /// <summary>
        ///     create a new op code
        /// </summary>
        /// <param name="id"></param>
        public OpCode(OpCodeId id) {
            Id = id;
            Params = ImmutableArray<byte>.Empty;
        }

        /// <summary>
        ///     create a new op code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="params"></param>
        public OpCode(OpCodeId id, ImmutableArray<byte> @params) {
            Id = id;
            Params = @params;
        }

        /// <summary>
        ///     op code ID
        /// </summary>
        public OpCodeId Id { get; }

        /// <summary>
        ///     parameters
        /// </summary>
        public ImmutableArray<byte> Params { get; }

    }
}
