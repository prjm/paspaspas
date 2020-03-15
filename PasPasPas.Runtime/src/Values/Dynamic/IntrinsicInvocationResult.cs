using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Dynamic {

    /// <summary>
    ///     result from an intrinsic routine call
    /// </summary>
    public class IntrinsicInvocationResult : IIntrinsicInvocationResult {

        /// <summary>
        ///     create a new intrinsic invocation result
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="targetRoutine"></param>
        public IntrinsicInvocationResult(IRoutineGroup targetRoutine, ISignature parameters) {
            Parameters = parameters;
            Routine = targetRoutine;
        }

        /// <summary>
        ///     parameters
        /// </summary>
        public ISignature Parameters { get; }

        /// <summary>
        ///     registered type id
        /// </summary>
        public ITypeDefinition TypeDefinition
            => Routine.DefiningType;

        /// <summary>
        ///     target routine
        /// </summary>
        public IRoutineGroup Routine { get; }

        /// <summary>
        ///     invocation result
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.InvocationResult;

    }
}