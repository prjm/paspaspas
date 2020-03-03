using System.Collections.Generic;
using PasPasPas.Globals.Environment;
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

        private static bool CanBeAssignedFromAlias(IAliasedType alias) {

            if (alias.BaseType == BaseType.Integer)
                return true;

            if (alias.BaseType == BaseType.Real)
                return true;

            if (alias.BaseType == BaseType.Char)
                return true;

            if (alias.BaseType == BaseType.Boolean)
                return true;

            if (alias.BaseType == BaseType.Enumeration)
                return true;

            return !alias.IsNewType;
        }

        /// <summary>
        ///     resolve type alias
        /// </summary>
        /// <param name="typeDefinition"></param>
        /// <returns></returns>
        public static ITypeDefinition ResolveAliasForAssignment(ITypeDefinition typeDefinition) {
            while (typeDefinition is IAliasedType alias && CanBeAssignedFromAlias(alias)) {
                typeDefinition = alias.BaseTypeDefinition;
            }

            return typeDefinition;
        }
        /*
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

*/
        /// <summary>
        ///     test if this type can be assigned from another type
        /// </summary>
        /// <param name="otherType">other type</param>
        /// <returns><c>true</c> if the type can be assigned from</returns>
        public virtual bool CanBeAssignedFromType(ITypeDefinition otherType) {

            if (otherType.IsErrorType())
                return false;

            if (otherType.Equals(this))
                return true;

            var baseType = ResolveAliasForAssignment(this);
            var anotherType = ResolveAliasForAssignment(otherType);

            if (baseType != this || anotherType != otherType) {
                return baseType.CanBeAssignedFromType(anotherType);
            }

            return false;
        }

        /// <summary>
        ///     helper function: get a list item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IPoolItem<List<T>> GetList<T>()
            => TypeRegistry.ListPools.GetList<T>();

    }
}
