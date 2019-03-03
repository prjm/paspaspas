using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     generic dynamic array type (<c>TArray&lt;T&gt;)</c>
    /// </summary>
    public class GenericArrayType : ArrayType, IGenericType {

        /// <summary>
        ///     create a new generic array type
        /// </summary>
        /// <param name="withId"></param>
        public GenericArrayType(int withId) : base(withId, KnownTypeIds.IntegerType) {
        }

        /// <summary>
        ///     reference type / pointer type
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetPointerSize();

        /// <summary>
        ///     number of generic type parameters
        /// </summary>
        public int NumberOfTypeParameters
            => 1;

        /// <summary>
        ///     bind to a generic type
        /// </summary>
        /// <param name="typeIds"></param>
        /// <returns></returns>
        public int Bind(ImmutableArray<int> typeIds) => throw new System.NotImplementedException();
    }
}
