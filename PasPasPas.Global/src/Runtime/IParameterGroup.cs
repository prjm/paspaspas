using System.Collections.Generic;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for parameter groups
    /// </summary>
    public interface IParameterGroup {

        /// <summary>
        ///     result type
        /// </summary>
        ITypeReference ResultType { get; }

        /// <summary>
        ///     parameters
        /// </summary>
        IList<IVariable> Parameters { get; }

        bool Matches(ITypeRegistry typeRegistry, Signature signature);
    }
}
