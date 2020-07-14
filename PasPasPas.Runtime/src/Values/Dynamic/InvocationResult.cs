using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Dynamic {

    /// <summary>
    ///     make a new invocation result
    /// </summary>
    public class InvocationResult : IInvocationResult, IEquatable<IInvocationResult> {

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
            => RoutineIndex.ResultType.TypeDefinition;

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

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ITypeSymbol? other)
            => Equals(other as IInvocationResult);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IInvocationResult? other)
            => RoutineIndex.Equals(other?.RoutineIndex) &&
                Routine.Equals(other?.Routine);
    }
}