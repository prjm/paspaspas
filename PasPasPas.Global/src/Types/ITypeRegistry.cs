using System.Collections.Generic;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     type registry
    /// </summary>
    public interface ITypeRegistry {

        /// <summary>
        ///     all registered types
        /// </summary>
        IEnumerable<ITypeDefinition> RegisteredTypeDefinitios { get; }

        /// <summary>
        ///     system unit
        /// </summary>
        IRefSymbol SystemUnit { get; }

        /// <summary>
        ///     make a type reference
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <returns>type reference</returns>
        ITypeReference MakeReference(int typeId);

        /// <summary>
        ///     get a type by type id
        /// </summary>
        /// <param name="typeId">given type id</param>
        /// <returns>type definition or the undefined type if the type id is not found</returns>
        ITypeDefinition GetTypeByIdOrUndefinedType(int typeId);

        /// <summary>
        ///     get an operator by operator id
        /// </summary>
        /// <param name="operatorKind">operator id</param>
        /// <returns>operator definition</returns>
        IOperator GetOperator(int operatorKind);

        /// <summary>
        ///     register an operator
        /// </summary>
        /// <param name="newOperator">operator to register</param>
        void RegisterOperator(IOperator newOperator);

        /// <summary>
        ///     register a new type
        /// </summary>
        /// <param name="typeDef"></param>
        ITypeDefinition RegisterType(ITypeDefinition typeDef);

        /// <summary>
        ///     runtime values, used to enable to calculate the results
        ///     of operators on constants
        /// </summary>
        IRuntimeValueFactory Runtime { get; set; }

        /// <summary>
        ///     generate a new user type id
        /// </summary>
        /// <returns></returns>
        int RequireUserTypeId();

        /// <summary>
        ///     get the type kind of a type id
        /// </summary>
        /// <param name="typeId">given type id</param>
        /// <returns>type kind</returns>
        CommonTypeKind GetTypeKindOf(int typeId);

        /// <summary>
        ///     gets the base type of a subrange type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        int GetBaseTypeOfSubrangeType(int typeId);

        /// <summary>
        ///     cast a type into a another type, if possible
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        int Cast(int sourceType, int targetType);

        /// <summary>
        ///     make a reference to a a type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        ITypeReference MakeTypeReference(int typeId);
    }
}
