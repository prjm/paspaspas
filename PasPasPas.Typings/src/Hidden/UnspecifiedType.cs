#nullable disable
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     unspecified type (placeholder)
    /// </summary>
    public class UnspecifiedType : HiddenIntrinsicType, IUnspecifiedType {

        /// <summary>
        ///     create a new unspecified type
        /// </summary>
        /// <param name="definingUnit"></param>
        public UnspecifiedType(IUnitType definingUnit) : base(definingUnit) {
        }
    }
}
