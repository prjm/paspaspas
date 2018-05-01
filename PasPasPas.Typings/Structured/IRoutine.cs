using System.Collections.Generic;
using PasPasPas.Global.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     interface for routines
    /// </summary>
    public interface IRoutine : IRefSymbol {

        /// <summary>
        ///     name of the routine
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     <c>true</c> for intrinsic constant methods
        /// </summary>
        bool IsConstant { get; }

        /// <summary>
        ///     resolve callable routines for a given signature
        /// </summary>
        /// <param name="callableRoutines">list to collect callable routines</param>
        /// <param name="signature">used signature</param>
        void ResolveCall(IList<ParameterGroup> callableRoutines, Signature signature);
    }
}
