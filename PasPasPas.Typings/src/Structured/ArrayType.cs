using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayType : StructuredTypeBase, IArrayType {

        /// <summary>
        ///     create a new array type
        /// </summary>
        /// <param name="withId"></param>
        public ArrayType(int withId) : base(withId) {
        }

        /// <summary>
        ///     array index types
        /// </summary>
        public ImmutableArray<int> IndexTypes { get; set; }

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ArrayType;

        /// <summary>
        ///     base type id
        /// </summary>
        public int BaseTypeId { get; set; }
            = KnownTypeIds.ErrorType;

        /// <summary>
        ///     base type id
        /// </summary>
        public ITypeDefinition BaseType
            => TypeRegistry.GetTypeByIdOrUndefinedType(BaseTypeId);

        /// <summary>
        ///     <c>true</c> if packed array
        /// </summary>
        public bool Packed { get; set; }
            = false;

        /// <summary>
        ///     check if the type can be assigned from another type
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.ArrayType && otherType is ArrayType array) {
                var isPackedString = (BaseType.TypeKind.IsChar() && array.BaseType.TypeKind.IsChar()) && (Packed && array.Packed);
                return isPackedString;
            }

            return base.CanBeAssignedFrom(otherType);
        }

        /// <summary>
        ///     convert this type to string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            var result = string.Empty;

            foreach (var index in IndexTypes) {
                result += $"[{index}] ";
            }
            return $"array {result}of {BaseType}".Trim();
        }
    }
}
