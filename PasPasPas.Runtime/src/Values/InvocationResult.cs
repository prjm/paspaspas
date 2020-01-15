using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     make a new invocation result
    /// </summary>
    public class InvocationResult : IInvocationResult, IEquatable<InvocationResult>, IEquatable<IntrinsicInvocationResult> {

        /// <summary>
        ///     create a new invocation result
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="routineIndex">routine index</param>
        public InvocationResult(IRoutineGroup routine, int routineIndex) {
            Routine = routine;
            RoutineIndex = routineIndex;
        }

        /// <summary>
        ///     result type
        /// </summary>
        public int TypeId {
            get {
                if (RoutineIndex < 0)
                    return KnownTypeIds.ErrorType;
                return Routine.Items[RoutineIndex].ResultType.TypeId;
            }
        }

        /// <summary>
        ///     internal type format
        /// </summary>
        public string InternalTypeFormat {
            get {
                if (RoutineIndex < 0)
                    return "#??";

                return $"#{Routine.Items[RoutineIndex].ResultType}";
            }
        }

        /// <summary>
        ///     invocation result
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.InvocationResult;

        /// <summary>
        ///     result type kind
        /// </summary>
        public CommonTypeKind TypeKind {
            get {
                if (RoutineIndex < 0)
                    return CommonTypeKind.UnknownType;

                return Routine.Items[RoutineIndex].ResultType.TypeKind;
            }
        }

        /// <summary>
        ///     referenced routine
        /// </summary>
        public IRoutineGroup Routine { get; }

        /// <summary>
        ///     routine index
        /// </summary>
        public int RoutineIndex { get; }

        /// <summary>
        ///     test if this result is a constant value
        /// </summary>
        public bool IsConstant {
            get {
                if (RoutineIndex < 0)
                    return false;

                return Routine.Items[RoutineIndex].ResultType.IsConstant();
            }
        }

        /// <summary>
        ///     compare
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(InvocationResult other)
            => Routine.Equals(other.Routine) && Routine.Items[RoutineIndex].Equals(other.Routine.Items[other.RoutineIndex]);

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is InvocationResult ir)
                return Equals(ir);

            if (obj is IntrinsicInvocationResult iir)
                return Equals(iir);

            return false;
        }

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IntrinsicInvocationResult other)
            => Routine.Equals(other.Routine) && Routine.Items[RoutineIndex].Equals(other.Parameters);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            unchecked {
                result = result * 31 + Routine.GetHashCode();
                result = result * 31 + RoutineIndex;
                return result;
            }
        }
    }
}