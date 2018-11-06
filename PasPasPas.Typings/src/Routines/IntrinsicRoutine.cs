using System.Collections.Generic;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     base class for intrinsic routines
    /// </summary>
    public abstract class IntrinsicRoutine : IRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => KnownTypeIds.IntrinsicRoutine;

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public abstract void ResolveCall(IList<ParameterGroup> callableRoutines, Signature signature);
    }
}
