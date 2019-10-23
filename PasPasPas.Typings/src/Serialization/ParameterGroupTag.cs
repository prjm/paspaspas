using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     tag for a parameter group
    /// </summary>
    internal class ParameterGroupTag : Tag {

        /// <summary>
        ///     tag kind: parameter group
        /// </summary>
        public override uint Kind
            => Constants.ParameterGroupTag;

        private ProcedureKind procedureKind;
        private ITypeReference resultType;

        internal override void ReadData(uint kind, TypeReader typeReader) {
            procedureKind = (ProcedureKind)typeReader.ReadByte();
        }

        internal override void WriteData(TypeWriter typeWriter) {
            typeWriter.WriteByte((byte)procedureKind);
        }

        internal void AddToRoutine(IRoutine routine)
            => routine.Parameters.Add(new ParameterGroup(routine, procedureKind, resultType));

        internal void Initialize(IParameterGroup parameterGroup)
            => procedureKind = parameterGroup.RoutineKind;
    }
}
