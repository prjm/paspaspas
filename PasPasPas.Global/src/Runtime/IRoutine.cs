﻿using System.Collections.Generic;
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
        ProcedureKind Kind { get; }

        /// <summary>
        ///     resolve callable routines for a given signature
        /// </summary>
        /// <param name="callableRoutines">list to collect callable routines</param>
        /// <param name="signature">used signature</param>
        void ResolveCall(IList<IParameterGroup> callableRoutines, Signature signature);


    }
}
