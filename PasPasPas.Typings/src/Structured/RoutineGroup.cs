using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     callable routine
    /// </summary>
    public class RoutineGroup : IRoutineGroup {

        /// <summary>
        ///     create a new routine
        /// </summary>
        public RoutineGroup(ITypeDefinition definingType, string name, ITypeDefinition genericType = default) {
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
            => default;

        /// <summary>
        ///     symbol type kind
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.RoutineGroup;

        /// <summary>
        ///     add a parameter group
        /// </summary>
        /// <param name="resultType">result type</param>
        /// <param name="kind">procedure kind</param>
        public Routine AddParameterGroup(RoutineKind kind, ITypeSymbol resultType) {
            var result = new Routine(this, kind, resultType);
            Items.Add(result);
            return result;
        }

        /// <summary>
        ///     add a parameter group
        /// </summary>
        /// <param name="firstParam">first parameter</param>
        /// <param name="resultType">result type</param>
        /// <param name="parameterName">parameter name</param>
        /// <param name="kind">procedure kind</param>
        /// <returns></returns>
        public Routine AddParameterGroup(string parameterName, RoutineKind kind, ITypeSymbol firstParam, ITypeSymbol resultType) {
            var result = new Routine(this, kind, resultType);
            result.AddParameter(parameterName).SymbolType = firstParam;
            Items.Add(result);
            return result;
        }

        /// <summary>
        ///     find a matching parameter group
        /// </summary>
        /// <param name="callableRoutines">list of callable routines</param>
        /// <param name="signature">used signature</param>
        public void ResolveCall(IList<IRoutine> callableRoutines, Signature signature) {
            foreach (var paramGroup in Items) {

                if (!paramGroup.Matches(default, signature))
                    continue;

                callableRoutines.Add(paramGroup);
            }
        }

    }
}
