#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Dynamic {

    /// <summary>
    ///     make a new invocation result
    /// </summary>
    public class InvocationResult : IInvocationResult {

        /// <summary>
        ///     create a new invocation result
        /// </summary>
        /// <param name="routine">routine index</param>
        public InvocationResult(IRoutine routine)
            => RoutineIndex = routine;

        /// <summary>
        ///     result type
        /// </summary>
        public ITypeDefinition TypeDefinition
            => Routine.DefiningType;

        /// <summary>
        ///     referenced routine
        /// </summary>
        public IRoutineGroup Routine
            => RoutineIndex.RoutineGroup;

        /// <summary>
        ///     symbol kind
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.InvocationResult;

        /// <summary>
        ///     called routine
        /// </summary>
        public IRoutine RoutineIndex { get; }

        IRoutine IInvocationResult.RoutineIndex => throw new System.NotImplementedException();
    }
}