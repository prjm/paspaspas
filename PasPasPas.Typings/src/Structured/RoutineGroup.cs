using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Globals;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     callable routine
    /// </summary>
    public class RoutineGroup : IRoutineGroup, IEquatable<IRoutineGroup> {

        /// <summary>
        ///     create a new routine
        /// </summary>
        public RoutineGroup(ITypeDefinition definingType, string name, ITypeDefinition? genericType = default) {
            Name = name;
            DefiningType = definingType;
        }

        /// <summary>
        ///     routine parameters
        /// </summary>
        public List<IRoutine> Items { get; }
            = new List<IRoutine>();

        /// <summary>
        ///     routine name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     defining type
        /// </summary>
        public ITypeDefinition DefiningType { get; }

        /// <summary>
        ///     no intrinsic routine
        /// </summary>
        public IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Unknown;

        /// <summary>
        ///     type definition
        /// </summary>
        public ITypeDefinition TypeDefinition
            => DefiningType.DefiningUnit.TypeRegistry.SystemUnit.RoutineGroupType;

        /// <summary>
        ///     symbol type kind
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.RoutineGroup;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ITypeSymbol? other)
            => Equals(other as IRoutineGroup);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IRoutineGroup? other)
            => RoutineId.Equals(other?.RoutineId) &&
                DefiningType.Equals(other?.DefiningType) &&
                Items.SequenceEqual(other?.Items);

        /// <summary>
        ///     find a matching parameter group
        /// </summary>
        /// <param name="callableRoutines">list of callable routines</param>
        /// <param name="signature">used signature</param>
        public void ResolveCall(IList<IRoutineResult> callableRoutines, ISignature signature) {
            foreach (var paramGroup in Items) {

                if (!paramGroup.Matches(default, signature))
                    continue;

                callableRoutines.Add(DefiningType.DefiningUnit.TypeRegistry.Runtime.Types.MakeInvocationResult(paramGroup));
            }
        }

    }
}
