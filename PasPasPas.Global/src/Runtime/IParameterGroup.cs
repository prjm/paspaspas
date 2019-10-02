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

        /// <summary>
        ///     <c>true</c> if this routine is a class item
        /// </summary>
        bool IsClassItem { get; }

        /// <summary>
        ///     check if the routine matches
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        bool Matches(ITypeRegistry typeRegistry, Signature signature);

        ////
        /// <summary>
        ///     create a signature
        /// </summary>
        /// <returns></returns>
        Signature CreateSignature(IRuntimeValueFactory runtime);
    }
}
