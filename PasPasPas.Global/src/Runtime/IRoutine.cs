using System.Collections.Generic;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {


    /// <summary>
    ///     generic interface for routines
    /// </summary>
    public interface IRoutine : IRefSymbol {

        /// <summary>
        ///     routine name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     procedure kind
        /// </summary>
        ProcedureKind Kind { get; }

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
        IList<IParameterGroup> Parameters { get; }

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
