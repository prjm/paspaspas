using System.Collections.Generic;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     generic interface for routine groups
    /// </summary>
    public interface IRoutineGroup : IRefSymbol, ITypeSymbol {

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
        void ResolveCall(IList<IRoutine> callableRoutines, Signature signature);

        /// <summary>
        ///     routines of this routine group
        /// </summary>
        List<IRoutine> Items { get; }

        /// <summary>
        ///     unique id for intrinsic routines
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
        public static int GetIndexOfParameterGroup(this IRoutineGroup routine, IRoutine group) {
            if (routine.Items == default)
                return -1;

            for (var i = 0; i < routine.Items.Count; i++)
                if (routine.Items[i].Equals(group))
                    return i;

            return -1;
        }

    }

}
