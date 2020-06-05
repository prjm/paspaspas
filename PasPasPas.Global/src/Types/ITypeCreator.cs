#nullable disable
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
        /// <param name="name">set name</param>
        /// <returns></returns>
        ISetType CreateSetType(IOrdinalType baseType, string name);

        /// <summary>
        ///     create a subrange type
        /// </summary>
        /// <param name="baseType">base type</param>
        /// <param name="lowerBound">lower bound</param>
        /// <param name="upperBound">upper bound</param>
        /// <param name="name">type name</param>
        /// <returns></returns>
        ISubrangeType CreateSubrangeType(string name, IOrdinalType baseType, IValue lowerBound, IValue upperBound);

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
        /// <param name="name">enumerated type name</param>
        /// <returns></returns>
        IEnumeratedType CreateEnumType(string name);

        /// <summary>
        ///     create a routine
        /// </summary>
        /// <param name="mainRoutineGroup"></param>
        /// <param name="procedure"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        IRoutine CreateRoutine(IRoutineGroup mainRoutineGroup, RoutineKind procedure, ISignature signature);

        /// <summary>
        ///     create a new structured type
        /// </summary>
        /// <param name="name">type name</param>
        /// <param name="typeKind">type kind</param>
        /// <returns></returns>
        IStructuredType CreateStructuredType(string name, StructuredTypeKind typeKind);

        /// <summary>
        ///     create a static array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="indexType">index type</param>
        /// <param name="isPacked"></param>
        /// <param name="name">type name</param>
        /// <returns></returns>
        IArrayType CreateStaticArrayType(ITypeDefinition baseType, string name, IOrdinalType indexType, bool isPacked);

        /// <summary>
        ///    create a new dynamic array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="typeName">type name</param>
        /// <param name="isPacked"><c>true</c> is the array is packed</param>
        /// <returns></returns>
        IArrayType CreateDynamicArrayType(ITypeDefinition baseType, string typeName, bool isPacked);

        /// <summary>
        ///     create a short string type
        /// </summary>
        /// <param name="length">string length</param>
        /// <returns></returns>
        IShortStringType CreateShortStringType(byte length);

        /// <summary>
        ///     create a new file type
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="baseTypeDefinition"></param>
        /// <returns></returns>
        IFileType CreateFileType(string typeName, ITypeDefinition baseTypeDefinition);

        /// <summary>
        ///     create a new generic type parameter
        /// </summary>
        /// <param name="constraints">type constraints</param>
        /// <param name="name">type name</param>
        /// <returns></returns>
        IGenericTypeParameter CreateUnboundGenericTypeParameter(string name, ImmutableArray<ITypeDefinition> constraints);

        /// <summary>
        ///     create a new generic type placeholder
        /// </summary>
        /// <param name="name">type name</param>
        /// <returns></returns>
        IExtensibleGenericType CreateGenericPlaceholder(string name);

        /// <summary>
        ///     create a routine type
        /// </summary>
        /// <returns></returns>
        IRoutineType CreateRoutineType(string name);

        /// <summary>
        ///     meta class type
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        IMetaType CreateMetaClassType(string name, ITypeDefinition baseType);
        IRoutineGroup CreateGlobalRoutineGroup(string routineName);
    }
}
