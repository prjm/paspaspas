﻿using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     make a new invocation result
    /// </summary>
    public class InvocationResult : IInvocationResult, IEquatable<InvocationResult> {

        /// <summary>
        ///     create a new invocation result
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="routineIndex">routine index</param>
        public InvocationResult(IRoutine routine, int routineIndex) {
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
                return Routine.Parameters[RoutineIndex].ResultType.TypeId;
            }
        }

        /// <summary>
        ///     internal type format
        /// </summary>
        public string InternalTypeFormat {
            get {
                if (RoutineIndex < 0)
                    return "#??";

                return $"#{Routine.Parameters[RoutineIndex].ResultType}";
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

                return Routine.Parameters[RoutineIndex].ResultType.TypeKind;
            }
        }

        /// <summary>
        ///     referenced routine
        /// </summary>
        public IRoutine Routine { get; }

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
                return Routine.Parameters[RoutineIndex].ResultType.IsConstant();
            }
        }

        /// <summary>
        ///     compare
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(InvocationResult other)
            => (other.TypeKind == TypeKind) && (other.TypeId == TypeId);
    }
}