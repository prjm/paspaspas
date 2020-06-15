using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     create a new void type
    /// </summary>
    internal class VoidType : HiddenIntrinsicType, INoType {

        /// <summary>
        ///     create a new void type
        /// </summary>
        /// <param name="definingUnit"></param>
        public VoidType(IUnitType definingUnit) : base(definingUnit) {
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(ITypeDefinition? other)
            => KnownNames.SameIdentifier(Name, other?.Name) &&
               other is INoType;
    }
}
