using System.Collections.Immutable;
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
        /// <param name="indexTypes"></param>
        public StaticArrayType(int withId, ImmutableArray<int> indexTypes) : base(withId, indexTypes) {
        }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                var result = 0u;

                foreach (var indexType in IndexTypes) {

                    var index = TypeRegistry.GetTypeByIdOrUndefinedType(indexType) as IOrdinalType;

                    if (index == default)
                        continue;

                    var lowerBound = index.LowestElement as IOrdinalValue;
                    var upperBound = index.HighestElement as IOrdinalValue;

                    if (lowerBound == default || upperBound == default)
                        continue;

                    var l = lowerBound.GetOrdinalValue(TypeRegistry);
                    var h = upperBound.GetOrdinalValue(TypeRegistry);
                    var size = TypeRegistry.Runtime.Integers.Subtract(h, l);

                    if (!size.IsIntegralValue(out var value))
                        continue;

                    if (value.SignedValue < 0)
                        continue;

                    var dimensionSize = (uint)(BaseType.TypeSizeInBytes * value.SignedValue);

                    if (result == 0u)
                        result = dimensionSize;
                    else
                        result *= dimensionSize;

                }

                return result;
            }
        }
    }
}
