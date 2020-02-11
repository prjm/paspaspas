using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Dynamic {

    /// <summary>
    ///     result from an intrinsic routine call
    /// </summary>
    public class IntrinsicInvocationResult : IIntrinsicInvocationResult, IEquatable<InvocationResult>, IEquatable<IntrinsicInvocationResult> {

        /// <summary>
        ///     create a new intrinsic invocation result
        /// </summary>
        /// <param name="parameterGroup"></param>
        /// <param name="targetRoutine"></param>
        public IntrinsicInvocationResult(IRoutineGroup targetRoutine, IRoutine parameterGroup) {
            Parameters = parameterGroup;
            Routine = targetRoutine;
        }

        /// <summary>
        ///     parameters
        /// </summary>
        public IRoutine Parameters { get; }

        /// <summary>
        ///     registered type id
        /// </summary>
        public ITypeDefinition TypeDefinition
            => Parameters.ResultType.TypeDefinition;

        /// <summary>
        ///     target routine
        /// </summary>
        public IRoutineGroup Routine { get; }

        /// <summary>
        ///     invocation result
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.InvocationResult;

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IntrinsicInvocationResult other)
            => Routine.Equals(other.Routine) && Parameters.Equals(other.Parameters);

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(InvocationResult other)
            => Routine.Equals(other.Routine) && Parameters.Equals(other.Routine.Items[other.RoutineIndex]);

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {

            if (obj is IntrinsicInvocationResult iir)
                return Equals(iir);

            if (obj is InvocationResult ir)
                return Equals(ir);

            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            unchecked {
                result = result * 31 + Parameters.GetHashCode();
                result = result * 31 + Routine.GetHashCode();
                return result;
            }
        }
    }
}