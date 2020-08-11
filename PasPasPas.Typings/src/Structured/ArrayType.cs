using System;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

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

        public override bool Equals(ITypeDefinition? other)
            => other is IArrayType a &&
                a.Kind == Kind &&
                a.IndexType.Equals(IndexType) &&
                a.BaseTypeDefinition.Equals(BaseTypeDefinition);

        public override int GetHashCode()
            => HashCode.Combine(Kind, IndexType, BaseTypeDefinition);


        /// <summary>
        ///     check if the type can be assigned from another type
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public override bool CanBeAssignedFromType(ITypeDefinition otherType) {

            if (otherType.BaseType == BaseType.Array && otherType is IArrayType array && array.Kind == ArrayTypeKind.StaticArray) {
                var isPackedString = BaseTypeDefinition.ResolveAlias().BaseType == BaseType.Char && array.BaseTypeDefinition.ResolveAlias().BaseType == BaseType.Char && Packed && array.Packed;
                return isPackedString;
            }

            return base.CanBeAssignedFromType(otherType);
        }

    }
}
