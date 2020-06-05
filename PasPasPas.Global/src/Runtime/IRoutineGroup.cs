using System.Collections.Generic;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     generic interface for routine groups
    /// </summary>
    public interface IRoutineGroup : INamedTypeSymbol {

        /// <summary>
        ///     defining type id
        /// </summary>
        ITypeDefinition DefiningType { get; }

        /// <summary>
        ///     resolve callable routines for a given signature
        /// </summary>
        /// <param name="callableRoutines">list to collect callable routines</param>
        /// <param name="signature">used signature</param>
        void ResolveCall(IList<IRoutineResult> callableRoutines, ISignature signature);

        /// <summary>
        ///     unique id for intrinsic routines
        /// </summary>
        IntrinsicRoutineId RoutineId { get; }

        /// <summary>
        ///     routine items
        /// </summary>
        List<IRoutine> Items { get; }
    }

}
