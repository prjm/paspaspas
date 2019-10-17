using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for parameter groups - describing a
    ///     routine by its parameters and properties
    /// </summary>
    public interface IParameterGroup {

        /// <summary>
        ///     routine kind
        /// </summary>
        ProcedureKind RoutineKind { get; }

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
        ///     parent routine
        /// </summary>
        IRoutine Routine { get; }

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
        Signature CreateSignature(ITypeRegistry runtime);

        /// <summary>
        ///     routine code
        /// </summary>
        ImmutableArray<IOpCode> Code { get; }

    }
}
