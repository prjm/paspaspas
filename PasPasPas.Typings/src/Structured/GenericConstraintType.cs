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
        /// <param name="kind">constraint kind</param>
        public GenericConstraintType(IUnitType definingUnit, GenericConstraintKind kind) : base(definingUnit)
            => Kind = kind;

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

        public GenericConstraintKind Kind { get; }
    }
}
