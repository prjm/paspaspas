using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     base class for type definitions
    /// </summary>
    public abstract class TypeDefinitionBase : ITypeDefinition {

        /// <summary>
        ///     create a new type definition
        /// </summary>
        /// <param name="definingUnit"></param>
        protected TypeDefinitionBase(IUnitType definingUnit)
            => DefiningUnit = definingUnit;

        /// <summary>
        ///     type definition
        /// </summary>
        public ITypeDefinition TypeDefinition
            => this;

        /// <summary>
        ///     get the base type
        /// </summary>
        public abstract BaseType BaseType { get; }

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry
            => DefiningUnit.TypeRegistry;

        /// <summary>
        ///     defining unit
        /// </summary>
        public IUnitType DefiningUnit { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public abstract uint TypeSizeInBytes { get; }

        /// <summary>
        ///     type name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     mangled type name
        /// </summary>
        public abstract string MangledName { get; }

        /// <summary>
        ///     type symbol kind
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.TypeDefinition;

        /*

        private static bool CanBeAssignedFromAlias(TypeAlias alias) {

            if (alias.TypeKind == CommonTypeKind.IntegerType)
                return true;

            if (alias.TypeKind == CommonTypeKind.Int64Type)
                return true;

            if (alias.TypeKind == CommonTypeKind.RealType)
                return true;

            if (alias.TypeKind == CommonTypeKind.AnsiCharType)
                return true;

            if (alias.TypeKind == CommonTypeKind.WideCharType)
                return true;

            if (alias.TypeKind == CommonTypeKind.BooleanType)
                return true;

            if (alias.TypeKind == CommonTypeKind.EnumerationType)
                return true;

            return !alias.IsNewType;
        }

        /// <summary>
        ///     resolve type alias
        /// </summary>
        /// <param name="typeDefinition"></param>
        /// <returns></returns>
        public static ITypeDefinition ResolveAliasForAssignment(ITypeDefinition typeDefinition) {
            while (typeDefinition is TypeAlias alias && CanBeAssignedFromAlias(alias)) {
                typeDefinition = alias.BaseType;
            }

            return typeDefinition;
        }

        /// <summary>
        ///     resolve type alias
        /// </summary>
        /// <param name="typeDefinition"></param>
        /// <returns></returns>
        public static ITypeDefinition ResolveAlias(ITypeDefinition typeDefinition) {
            while (typeDefinition is TypeAlias alias) {
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
            if (otherType.TypeId.In(KnownTypeIds.ErrorType, KnownTypeIds.NoType, KnownTypeIds.UnspecifiedType))
                return false;

            if (otherType.TypeId == TypeInfo.TypeId)
                return true;

            var baseType = ResolveAliasForAssignment(this);
            var anotherType = ResolveAliasForAssignment(otherType);

            if (baseType != this || anotherType != otherType) {
                return baseType.CanBeAssignedFrom(anotherType);
            }

            return false;
        }

        /// <summary>
        ///     get the short type name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => ShortName;

        /// <summary>
        ///     helper function: get a list item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IPoolItem<List<T>> GetList<T>()
            => TypeRegistry.ListPools.GetList<T>();
    */
    }
}
