using System.Collections.Immutable;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     dynamic array type
    /// </summary>
    public class DynamicArrayType : ArrayType {

        /// <summary>
        ///     create a new dynamic array type
        /// </summary>
        /// <param name="withId"></param>
        public DynamicArrayType(int withId) : base(withId, ImmutableArray<int>.Empty) {
        }

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.NativeInt).TypeSizeInBytes;
    }
}
