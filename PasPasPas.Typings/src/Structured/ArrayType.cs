#nullable disable
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     array type definition
    /// </summary>
    public abstract class ArrayType : StructuredTypeBase, IArrayType {

        /// <summary>
        ///     create a new array type
        /// </summary>
        /// <param name="definingUnit"></param>
        /// <param name="indexType">index types</param>
        protected ArrayType(IUnitType definingUnit, ITypeDefinition indexType) : base(definingUnit)
            => IndexType = indexType;

        /// <summary>
        ///     base type kind
        /// </summary>
        public override BaseType BaseType
            => BaseType.Array;

        /// <summary>
        ///     type kind
        /// </summary>
        public abstract ArrayTypeKind Kind { get; }

        /// <summary>
        ///     array index types
        /// </summary>
        public ITypeDefinition IndexType { get; }

        /// <summary>
        ///     base type id
        /// </summary>
        public ITypeDefinition BaseTypeDefinition { get; set; }
            = default;

        /// <summary>
        ///     <c>true</c> if packed array
        /// </summary>
        public bool Packed { get; set; }
            = false;

        /// <summary>
        ///     mangled name
        /// </summary>
        public override string MangledName
            => string.Concat(DefiningUnit.Name, KnownNames.AtSymbol, Name);

        /*
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
        */
    }
}
