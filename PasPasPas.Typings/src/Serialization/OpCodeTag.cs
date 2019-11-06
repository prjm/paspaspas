using System;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Typings.OpCodes;

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

        /// <summary>
        ///     operation code
        /// </summary>
        public IOpCode OpCode { get; private set; }

        /// <summary>
        ///     read the tag
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="typeReader"></param>
        internal override void ReadData(uint kind, TypeReader typeReader) => throw new System.NotImplementedException();

        /// <summary>
        ///     write the tag
        /// </summary>
        /// <param name="typeWriter"></param>
        internal override void WriteData(TypeWriter typeWriter) {
            typeWriter.WriteByte(OpCode.Id);

            switch (OpCode) {
                case CallOpCode call:
                    WriteCallOpCode(call);
                    break;

                default:
                    throw new InvalidOperationException(OpCode.OpCodeText);
            }
        }

        private void WriteCallOpCode(CallOpCode call)
            => throw new NotImplementedException();

        /// <summary>
        ///     initialize with a given op code
        /// </summary>
        /// <param name="op"></param>
        internal void Initialize(IOpCode op)
            => OpCode = op;
    }
}
