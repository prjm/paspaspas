using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for the global type registry
    /// </summary>
    public interface ITypeRegistry {

        /// <summary>
        ///     system unit
        /// </summary>
        ISystemUnit SystemUnit { get; }

        /// <summary>
        ///     get an operator by operator id
        /// </summary>
        /// <param name="operatorKind">operator id</param>
        /// <returns>operator definition</returns>
        IOperator GetOperator(OperatorKind operatorKind);

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
        ///     list pools
        /// </summary>
        IListPools ListPools { get; }

        /// <summary>
        ///     registered units
        /// </summary>
        IEnumerable<IUnitType> Units { get; }

        /// <summary>
        ///     find a intrinsic routine by id
        /// </summary>
        /// <param name="routineId">routine to find</param>
        /// <returns>found intrinsic routine</returns>
        IRoutineGroup GetIntrinsicRoutine(IntrinsicRoutineId routineId);

        /// <summary>
        ///     type factory
        /// </summary>
        ITypeCreator CreateTypeFactory(IUnitType unitType);

        /// <summary>
        ///     cast a type symbol to another type symbol
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        /// <returns></returns>
        ITypeSymbol Cast(ITypeSymbol fromType, ITypeSymbol toType);

        /// <summary>
        ///     create a new unit type
        /// </summary>
        /// <returns></returns>
        IUnitType CreateUnitType(string name);
    }
}
