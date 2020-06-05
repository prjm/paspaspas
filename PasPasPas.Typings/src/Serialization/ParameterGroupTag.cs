#nullable disable
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

        private RoutineKind procedureKind;
        private ITypeSymbol resultType;

        internal override void ReadData(uint kind, TypeReader typeReader) {
            procedureKind = (RoutineKind)typeReader.ReadByte();
        }

        internal override void WriteData(TypeWriter typeWriter) {
            typeWriter.WriteByte((byte)procedureKind);
        }

        internal void AddToRoutine(IRoutineGroup routine)
            => routine.Items.Add(ToParameterGroup(routine));

        internal IRoutine ToParameterGroup(IRoutineGroup routine)
            => new Routine(routine, procedureKind, routine.TypeDefinition.DefiningUnit.TypeRegistry.Runtime.Types.MakeSignature(resultType));

        internal void Initialize(IRoutine parameterGroup)
            => procedureKind = parameterGroup.Kind;
    }
}
