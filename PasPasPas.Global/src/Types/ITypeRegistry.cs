using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for the global type registry
    /// </summary>
    public interface ITypeRegistry : IEnvironmentItem {

        /// <summary>
        ///     all registered types
        /// </summary>
        IEnumerable<ITypeDefinition> RegisteredTypeDefinitions { get; }

        /// <summary>
        ///     system unit
        /// </summary>
        ISystemUnit SystemUnit { get; }

        /// <summary>
        ///     byte type
        /// </summary>
        IIntegralType ByteType { get; }

        /// <summary>
        ///     short int type
        /// </summary>
        IIntegralType ShortIntType { get; }

        /// <summary>
        ///     make a type reference
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <returns>type reference</returns>
        IOldTypeReference MakeTypeInstanceReference(int typeId);

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
        ///     runtime values, used to enable to calculate the results
        ///     of operators on constants
        /// </summary>
        IRuntimeValueFactory Runtime { get; }

        /// <summary>
        ///     get the type kind of a type id
        /// </summary>
        /// <param name="typeId">given type id</param>
        /// <returns>type kind</returns>
        CommonTypeKind GetTypeKindOf(int typeId);

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
        IOldTypeReference MakeTypeReference(int typeId);

        /// <summary>
        ///     list pools
        /// </summary>
        IListPools ListPools { get; }

        /// <summary>
        ///     find a intrinsic routine by id
        /// </summary>
        /// <param name="routineId">routine to find</param>
        /// <returns>found intrinsic routine</returns>
        IRoutineGroup GetIntrinsicRoutine(IntrinsicRoutineId routineId);

        /// <summary>
        ///     type factory
        /// </summary>
        ITypeCreator TypeCreator { get; }

    }
}
