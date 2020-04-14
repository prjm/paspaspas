﻿using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for a routine definition
    /// </summary>
    public interface IRoutine {

        /// <summary>
        ///     routine kind
        /// </summary>
        RoutineKind Kind { get; }

        /// <summary>
        ///     result type
        /// </summary>
        ITypeSymbol ResultType { get; }

        /// <summary>
        ///     parameters
        /// </summary>
        IList<IVariable> Parameters { get; }

        /// <summary>
        ///     parent routine group
        /// </summary>
        IRoutineGroup RoutineGroup { get; }

        /// <summary>
        ///     check if the routine matches
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        bool Matches(ITypeRegistry typeRegistry, ISignature signature);

        /// <summary>
        ///     routine code
        /// </summary>
        ImmutableArray<OpCode> Code { get; }

        /// <summary>
        ///     other symbols of this routine
        /// </summary>
        IDictionary<string, ITypeSymbol> Symbols { get; }

        /// <summary>
        ///     routine flags
        /// </summary>
        RoutineFlags Flags { get; }

    }

    /// <summary>
    ///     helper functions for routines
    /// </summary>
    public static class RoutineFlagsHelper {

        /// <summary>
        ///     test if the routine is a class routine
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public static bool IsClassItem(this IRoutine routine)
            => (routine.Flags & RoutineFlags.ClassItem) == RoutineFlags.ClassItem;
    }

}
