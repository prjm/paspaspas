using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     hidden type
    /// </summary>
    internal abstract class HiddenIntrinsicType : TypeDefinitionBase {

        /// <summary>
        ///     create a new hidden type
        /// </summary>
        /// <param name="definingUnit"></param>
        internal HiddenIntrinsicType(IUnitType definingUnit) : base(definingUnit) {
        }

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;

        /// <summary>
        ///     base type: error type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Error;

    }
}