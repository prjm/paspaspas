using PasPasPas.Globals.Runtime;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     hidden type
    /// </summary>
    public class HiddenIntrinsicType : TypeBase {

        /// <summary>
        ///     create a new hidden type
        /// </summary>
        /// <param name="withId"></param>
        public HiddenIntrinsicType(int withId) : base(withId) {
        }

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.HiddenType;
    }
}