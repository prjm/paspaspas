using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     nil type
    /// </summary>
    internal class NilType : HiddenIntrinsicType {

        /// <summary>
        ///     create a new void type
        /// </summary>
        /// <param name="definingUnit"></param>
        internal NilType(IUnitType definingUnit) : base(definingUnit) {
        }

        public override bool Equals(ITypeDefinition? other)
            => other is NilType;
    }
}
