using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for the global type registry
    /// </summary>
    public interface ITypeRegistry : IEnvironmentItem {

        /// <summary>
        ///     system unit
        /// </summary>
        ISystemUnit SystemUnit { get; }

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
        ///     cast a type into a another type, if possible
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        int Cast(int sourceType, int targetType);

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
        ITypeCreator CreateTypeFactory(IUnitType unitType);

    }
}
