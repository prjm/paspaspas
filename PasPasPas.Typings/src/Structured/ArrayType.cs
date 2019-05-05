using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     array type definition
    /// </summary>
    public abstract class ArrayType : StructuredTypeBase, IArrayType {

        /// <summary>
        ///     create a new array type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="indexType">index types</param>
        protected ArrayType(int withId, int indexType) : base(withId)
            => IndexTypeId = indexType;

        /// <summary>
        ///     array index types
        /// </summary>
        public int IndexTypeId { get; }

        /// <summary>
        ///     index type
        /// </summary>
        public ITypeDefinition IndexType
            => TypeRegistry.GetTypeByIdOrUndefinedType(IndexTypeId);

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

            if (otherType.TypeKind == CommonTypeKind.StaticArrayType && otherType is ArrayType array) {
                var isPackedString = BaseType.TypeKind.IsChar() && array.BaseType.TypeKind.IsChar() && Packed && array.Packed;
                return isPackedString;
            }

            return base.CanBeAssignedFrom(otherType);
        }

    }
}
