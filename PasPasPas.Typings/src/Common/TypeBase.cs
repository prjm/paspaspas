using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     basic type definition
    /// </summary>
    public abstract class TypeBase : ITypeDefinition {

        /// <summary>
        ///     create a new type definition
        /// </summary>
        /// <param name="withId">type id</param>
        protected TypeBase(int withId)
            => TypeId = withId;

        /// <summary>
        ///     get the type id
        /// </summary>
        public ITypeReference TypeInfo
            => TypeRegistry.MakeTypeInstanceReference(TypeId);

        /// <summary>
        ///     get the type kind
        /// </summary>
        public abstract CommonTypeKind TypeKind { get; }

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     registered type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public abstract uint TypeSizeInBytes { get; }

        /// <summary>
        ///     short type name
        /// </summary>
        public virtual string ShortName
            => "";

        /// <summary>
        ///     long type name
        /// </summary>
        public virtual string LongName
            => "";

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
    }
}
