using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     generic constraint type
    /// </summary>
    public class GenericConstraintType : TypeDefinitionBase {

        /// <summary>
        ///     create a new generic constraint type
        /// </summary>
        /// <param name="definingUnit"></param>
        public GenericConstraintType(IUnitType definingUnit) : base(definingUnit) {
        }

        /// <summary>
        ///     hidden type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Hidden;


        /// <summary>
        ///     invisible type
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;

        /// <summary>
        ///     type name
        /// </summary>
        public override string Name
            => string.Empty;

        /// <summary>
        ///     mangled type name
        /// </summary>
        public override string MangledName
            => string.Empty;
    }
}
