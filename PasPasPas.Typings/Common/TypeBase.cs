﻿using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     basic type definition
    /// </summary>
    public abstract class TypeBase : ITypeDefinition {

        private readonly int typeId;

        /// <summary>
        ///     create a new type definiton
        /// </summary>
        /// <param name="withId">type id</param>
        public TypeBase(int withId)
            => typeId = withId;

        /// <summary>
        ///     get the type id
        /// </summary>
        public int TypeId => typeId;

        /// <summary>
        ///     get the type kind
        /// </summary>
        public abstract CommonTypeKind TypeKind { get; }

        /// <summary>
        ///     type registryB
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        private bool CanBeAssignedFromAlias(TypeAlias alias) {

            if (alias.TypeKind == CommonTypeKind.IntegerType)
                return true;

            if (alias.TypeKind == CommonTypeKind.Int64Type)
                return true;

            if (alias.TypeKind == CommonTypeKind.FloatType)
                return true;

            if (alias.TypeKind == CommonTypeKind.AnsiCharType)
                return true;

            if (alias.TypeKind == CommonTypeKind.WideCharType)
                return true;

            return !alias.IsNewType;
        }

        private ITypeDefinition ResolveAlias(ITypeDefinition typeDefinition) {
            while (typeDefinition is TypeAlias alias && CanBeAssignedFromAlias(alias)) {
                typeDefinition = alias.BaseType;
            }

            return typeDefinition;
        }

        /// <summary>
        ///     test if this type can be assigned from another type
        /// </summary>
        /// <param name="otherType">other type</param>
        /// <returns><c>true</c> if the type can be assigned from</returns>
        public virtual bool CanBeAssignedFrom(ITypeDefinition otherType) {
            if (otherType.TypeId.In(TypeIds.ErrorType, TypeIds.NoType, TypeIds.UnspecifiedType))
                return false;

            if (otherType.TypeId == TypeId)
                return true;

            var baseType = ResolveAlias(this);
            var anotherType = ResolveAlias(otherType);

            if (baseType != this || anotherType != otherType) {
                return baseType.CanBeAssignedFrom(anotherType);
            }

            return false;
        }
    }
}
