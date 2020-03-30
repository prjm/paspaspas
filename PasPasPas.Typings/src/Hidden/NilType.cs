using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     nil type
    /// </summary>
    public class NilType : HiddenIntrinsicType {
        /// <summary>
        ///     create a new void type
        /// </summary>
        /// <param name="definingUnit"></param>
        public NilType(IUnitType definingUnit) : base(definingUnit) {
        }
    }
}
