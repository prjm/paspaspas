using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     procedural type
    /// </summary>
    public class RoutineType : TypeBase, IRoutineType {

        /// <summary>
        ///     create a new routine type
        /// </summary>
        /// <param name="typeId"></param>
        public RoutineType(int typeId) : base(typeId) { }

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ProcedureType;

        /// <summary>
        ///     undefined type size
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;

        /// <summary>
        ///     generic parameters
        /// </summary>
        public List<int> GenericParameters
            => new List<int>();

        /// <summary>
        ///     number of generic type parameters
        /// </summary>
        public int NumberOfTypeParameters
            => GenericParameters.Count;

        /// <summary>
        ///     add a generic parameter
        /// </summary>
        /// <param name="typeId"></param>
        public void AddGenericParameter(int typeId)
            => GenericParameters.Add(typeId);

        /// <summary>
        ///     bind the generic type parameter
        /// </summary>
        /// <param name="typeIds"></param>
        /// <returns></returns>
        public Reference Bind(ImmutableArray<int> typeIds)
            => default;
    }
}
