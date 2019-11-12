using System;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.OpCodes;
using PasPasPas.Typings.Routines;

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
        public IOpCode OpCode { get; private set; }

        /// <summary>
        ///     read the tag
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="typeReader"></param>
        internal override void ReadData(uint kind, TypeReader typeReader) {
            var opcodeId = typeReader.ReadByte().ToOpCodeId();

            switch (opcodeId) {

                case OpCodeId.Call:
                    ReadCallOpCode(typeReader);
                    break;

                default:
                    throw new InvalidOperationException(opcodeId.ToString());

            }
        }

        /// <summary>
        ///     write the tag
        /// </summary>
        /// <param name="typeWriter"></param>
        internal override void WriteData(TypeWriter typeWriter) {
            typeWriter.WriteByte(OpCode.Id.ToByte());

            switch (OpCode) {
                case CallOpCode call:
                    WriteCallOpCode(typeWriter, call);
                    break;

                default:
                    throw new InvalidOperationException(OpCode.OpCodeText);
            }
        }

        private void WriteCallOpCode(TypeWriter typeWriter, CallOpCode call) {
            var invocResult = call.CallInfo;

            if (invocResult.Routine.RoutineId == Globals.Runtime.IntrinsicRoutineId.Unknown)
                throw new InvalidOperationException();

            var iroutine = invocResult.Routine as IntrinsicRoutine;
            var iresult = invocResult as IIntrinsicInvocationResult;
            typeWriter.WriteByte(iroutine.RoutineId.ToByte());
            parms.Initialize(iresult.Parameters);
            typeWriter.WriteTag(parms);
        }

        private void ReadCallOpCode(TypeReader reader) {
            var routineId = reader.ReadByte().ToIntrinsicRoutineId();

            if (routineId == IntrinsicRoutineId.Unknown)
                throw new InvalidOperationException();

            var routine = reader.Types.GetIntrinsicRoutine(routineId);
            reader.ReadTag(parms);
            var invocResult = reader.Types.Runtime.Types.MakeInvocationResultFromIntrinsic(routine, parms.ToParameterGroup(routine));
            OpCode = new CallOpCode(invocResult);
        }

        /// <summary>
        ///     initialize with a given op code
        /// </summary>
        /// <param name="op"></param>
        internal void Initialize(IOpCode op)
            => OpCode = op;
    }
}
