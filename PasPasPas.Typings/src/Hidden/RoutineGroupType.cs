using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     common routine group type
    /// </summary>
    internal class RoutineGroupType : HiddenIntrinsicType, IRoutineGroupType {

        public RoutineGroupType(IUnitType definingUnit) : base(definingUnit) {
        }

        public override bool Equals(ITypeDefinition? other)
            => other is IRoutineGroupType;

        public override int GetHashCode()
            => 0;
    }
}
