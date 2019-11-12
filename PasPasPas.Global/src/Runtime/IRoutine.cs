﻿using System.Collections.Generic;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     generic interface for routine groups
    /// </summary>
    public interface IRoutine : IRefSymbol {

        /// <summary>
        ///     routine name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     defining type id
        /// </summary>
        int DefiningType { get; }

        /// <summary>
        ///     resolve callable routines for a given signature
        /// </summary>
        /// <param name="callableRoutines">list to collect callable routines</param>
        /// <param name="signature">used signature</param>
        void ResolveCall(IList<IParameterGroup> callableRoutines, Signature signature);

        /// <summary>
        ///     parameters
        /// </summary>
        List<IParameterGroup> Parameters { get; }

        /// <summary>
        ///     <c>true</c> for intrinsic routines
        /// </summary>
        IntrinsicRoutineId RoutineId { get; }

    }


    /// <summary>
    ///     helper methods
    /// </summary>
    public static class IRoutineHelpers {

        /// <summary>
        ///     get the index of a given parameter group
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static int GetIndexOfParameterGroup(this IRoutine routine, IParameterGroup group) {
            if (routine.Parameters == default)
                return -1;

            for (var i = 0; i < routine.Parameters.Count; i++)
                if (routine.Parameters[i].Equals(group))
                    return i;

            return -1;
        }

    }

}
