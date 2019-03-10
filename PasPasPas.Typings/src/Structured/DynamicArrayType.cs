using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     dynamic array type
    /// </summary>
    public class DynamicArrayType : ArrayType {

        /// <summary>
        ///     create a new dynamic array type
        /// </summary>
        /// <param name="withId"></param>
        public DynamicArrayType(int withId) : base(withId, KnownTypeIds.IntegerType) {
        }

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetPointerSize();


        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.DynamicArrayType;

        /// <summary>
        ///     convert this type to string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            var result = $"[{IndexType}] ";
            return $"dynamic array {result}of {BaseType}".Trim();
        }


    }
}
