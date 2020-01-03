using System.Collections.Immutable;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     op code tag
    /// </summary>
    internal class OpCodeTag : Tag {

        /// <summary>
        ///     op code tag
        /// </summary>
        public override uint Kind
            => Constants.OpCodeTag;

        private ParameterGroupTag parms
            = new ParameterGroupTag();

        /// <summary>
        ///     operation code
        /// </summary>
        public OpCode OpCode { get; private set; }

        /// <summary>
        ///     read the tag
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="typeReader"></param>
        internal override void ReadData(uint kind, TypeReader typeReader) {
            var opcodeId = typeReader.ReadByte().ToOpCodeId();
            var paramLength = (int)typeReader.ReadUint();
            var parms = new byte[paramLength];
            for (var i = 0; i < paramLength; i++)
                parms[i] = typeReader.ReadByte();

            OpCode = new OpCode(opcodeId, ImmutableArray.Create(parms));
        }

        /// <summary>
        ///     write the tag
        /// </summary>
        /// <param name="typeWriter"></param>
        internal override void WriteData(TypeWriter typeWriter) {
            typeWriter.WriteByte(OpCode.Id.ToByte());
            typeWriter.WriteUint((uint)OpCode.Params.Length);
            for (var i = 0; i < OpCode.Params.Length; i++)
                typeWriter.WriteByte(OpCode.Params[i]);
        }

        /// <summary>
        ///     initialize with a given op code
        /// </summary>
        /// <param name="op"></param>
        internal void Initialize(OpCode op)
            => OpCode = op;
    }
}
