using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     generic type parameter
    /// </summary>
    public class GenericTypeParameter : StructuredTypeBase, IGenericTypeParameter {

        /// <summary>
        ///     create a new generic type parameter
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="constraints"></param>
        public GenericTypeParameter(int typeId, ImmutableArray<int> constraints) : base(typeId)
            => Constraints = constraints;

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.Type;

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;

        /// <summary>
        ///     type constraints
        /// </summary>
        public ImmutableArray<int> Constraints { get; }
    }
}
