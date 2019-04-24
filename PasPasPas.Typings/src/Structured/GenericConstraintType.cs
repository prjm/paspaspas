using PasPasPas.Globals.Runtime;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     generic constraint type
    /// </summary>
    public class GenericConstraintType : TypeBase {

        /// <summary>
        ///     create a new generic constraint typwe
        /// </summary>
        /// <param name="withId"></param>
        public GenericConstraintType(int withId) : base(withId) {
        }

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.HiddenType;

        /// <summary>
        ///     invisible type
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;
    }
}
