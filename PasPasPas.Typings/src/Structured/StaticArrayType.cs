using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     static array type
    /// </summary>
    public class StaticArrayType : ArrayType {

        /// <summary>
        ///     create a new static array type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="indexType"></param>
        public StaticArrayType(int withId, int indexType) : base(withId, indexType) {
        }

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.StaticArrayType;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                var index = TypeRegistry.GetTypeByIdOrUndefinedType(IndexTypeId) as IOrdinalType;

                var lowerBound = index.LowestElement as IOrdinalValue;
                var upperBound = index.HighestElement as IOrdinalValue;

                if (lowerBound == default || upperBound == default)
                    return 0;

                var l = lowerBound.GetOrdinalValue(TypeRegistry);
                var h = upperBound.GetOrdinalValue(TypeRegistry);
                var size = TypeRegistry.Runtime.Integers.Subtract(h, l);
                var value = size as IIntegerValue;
                return (uint)(BaseType.TypeSizeInBytes * value.SignedValue);
            }
        }

        /// <summary>
        ///     convert this type to string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            var result = $"[{IndexType}] ";
            return $"static array {result}of {BaseType}".Trim();
        }

    }
}
