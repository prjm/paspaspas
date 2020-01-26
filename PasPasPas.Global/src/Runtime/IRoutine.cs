using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for a routine definition
    /// </summary>
    public interface IRoutine {

        /// <summary>
        ///     routine kind
        /// </summary>
        RoutineKind Kind { get; }

        /// <summary>
        ///     result type
        /// </summary>
        ITypeSymbol ResultType { get; }

        /// <summary>
        ///     parameters
        /// </summary>
        IList<IVariable> Parameters { get; }

        /// <summary>
        ///     <c>true</c> if this routine is a class item
        /// </summary>
        [Obsolete("to be replaced")]
        bool IsClassItem { get; }

        /// <summary>
        ///     parent routine
        /// </summary>
        IRoutineGroup RoutineGroup { get; }

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
        ImmutableArray<OpCode> Code { get; }

        /// <summary>
        ///     other symbols of this routine
        /// </summary>
        IDictionary<string, Reference> Symbols { get; }

    }
}
