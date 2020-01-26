﻿using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     result of an invocation
    /// </summary>
    public interface IInvocationResult : ITypeSymbol {

        /// <summary>
        ///     get the called routine
        /// </summary>
        IRoutineGroup Routine { get; }

    }
}
