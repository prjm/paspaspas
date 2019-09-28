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

        /// <summary>
        ///     procedure kind
        /// </summary>
        ProcedureKind Kind { get; }

        /// <summary>
        ///     defining type id
        /// </summary>
        int DefiningType { get; }

        /// <summary>
        ///     check if this routine is a class item
        /// </summary>
        bool IsClassItem { get; }

        /// <summary>
        ///     create a signature for this routine
        /// </summary>
        /// <returns></returns>
        Signature CreateSignature();

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


}
