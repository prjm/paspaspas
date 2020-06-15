using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     array type definition
    /// </summary>
    internal abstract class ArrayType : StructuredTypeBase, IArrayType {

        /// <summary>
        ///     create a new array type
        /// </summary>
        /// <param name="definingUnit"></param>
        /// <param name="indexType">index types</param>
        /// <param name="baseTypeDefinition">base type definition</param>
        protected ArrayType(IUnitType definingUnit, ITypeDefinition indexType, ITypeDefinition baseTypeDefinition) : base(definingUnit) {
            IndexType = indexType;
            BaseTypeDefinition = baseTypeDefinition;
        }

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
        public ITypeDefinition BaseTypeDefinition { get; }

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

        public override bool Equals(ITypeDefinition? other)
            => KnownNames.SameIdentifier(Name, other?.Name) &&
                other is IArrayType a &&
                a.Kind == Kind &&
                a.IndexType.Equals(IndexType);

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
