using System;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     generic constraint type
    /// </summary>
    internal class GenericConstraintType : TypeDefinitionBase {

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

        public GenericConstraintKind Kind { get; }

        public override bool Equals(ITypeDefinition? other)
            => other is GenericConstraintType g &&
                g.Kind == Kind;

        public override int GetHashCode()
            => HashCode.Combine(Kind);
    }
}
