using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     factory for new types
    /// </summary>
    public interface ITypeCreator {

        /// <summary>
        ///     create a new set type
        /// </summary>
        /// <param name="baseType">base type for the set (ordinal type)</param>
        /// <returns></returns>
        ISetType CreateSetType(IOrdinalType baseType);

        /// <summary>
        ///     create a subrange type
        /// </summary>
        /// <param name="baseType">base type</param>
        /// <param name="lowerBound">lower bound</param>
        /// <param name="upperBound">upper bound</param>
        /// <returns></returns>
        ISubrangeType CreateSubrangeType(IOrdinalType baseType, IValue lowerBound, IValue upperBound);

        /// <summary>
        ///     create a new aliased type
        /// </summary>
        /// <param name="baseType">base type id</param>
        /// <param name="newType"><c>true</c> if the type should be treated as a new type</param>
        /// <param name="aliasName">alias name</param>
        /// <returns></returns>
        IAliasedType CreateTypeAlias(ITypeDefinition baseType, string aliasName, bool newType);

        /// <summary>
        ///     create a new enumerated type
        /// </summary>
        /// <returns></returns>
        IEnumeratedType CreateEnumType();

        /// <summary>
        ///     create a new structured type
        /// </summary>
        /// <param name="typeKind"></param>
        /// <returns></returns>
        IStructuredType CreateStructuredType(StructuredTypeKind typeKind);

        /// <summary>
        ///     create a static array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="indexType">index type</param>
        /// <param name="isPacked"></param>
        /// <returns></returns>
        IArrayType CreateStaticArrayType(ITypeDefinition baseType, IOrdinalType indexType, bool isPacked);

        /// <summary>
        ///    create a new dynamic array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="isPacked"><c>true</c> is the array is packed</param>
        /// <returns></returns>
        IArrayType CreateDynamicArrayType(int baseType, bool isPacked);

        /// <summary>
        ///     create a short string type
        /// </summary>
        /// <param name="length">string length</param>
        /// <returns></returns>
        IShortStringType CreateShortStringType(byte length);

        /// <summary>
        ///     create a new unit type
        /// </summary>
        /// <returns></returns>
        IUnitType CreateUnitType(string name);

        /// <summary>
        ///     create a new file type
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        IFileType CreateFileType(ITypeDefinition baseType);

        /// <summary>
        ///     create a new generic type parameter
        /// </summary>
        /// <param name="constraints">type constraints</param>
        /// <returns></returns>
        IGenericTypeParameter CreateUnboundGenericTypeParameter(ImmutableArray<int> constraints);

        /// <summary>
        ///     create a new generic type placeholder
        /// </summary>
        /// <returns></returns>
        IExtensibleGenericType CreateGenericPlaceholder();

        /// <summary>
        ///     create a routine type
        /// </summary>
        /// <returns></returns>
        IRoutineType CreateRoutineType();

        /// <summary>
        ///     meta class type
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        IMetaClassType CreateMetaClassType(ITypeDefinition baseType);
    }
}
