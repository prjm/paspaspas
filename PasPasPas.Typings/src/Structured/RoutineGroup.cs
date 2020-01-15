using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     callable routine
    /// </summary>
    public class RoutineGroup : IRoutineGroup, ITypeReference {

        /// <summary>
        ///     create a new routine
        /// </summary>
        /// <param name="name">routine name</param>
        /// <param name="types">type registry</param>
        /// <param name="definingType">defining type</param>
        /// <param name="genericTypeId">generic type id</param>
        public RoutineGroup(ITypeRegistry types, string name, int genericTypeId = KnownTypeIds.ErrorType, int definingType = KnownTypeIds.ErrorType) {
            Name = name;
            TypeRegistry = types;
            TypeId = genericTypeId;
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
        ///     used type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     defining type
        /// </summary>
        public int DefiningType { get; }
            = KnownTypeIds.UnspecifiedType;

        /// <summary>
        ///     internal type format
        /// </summary>
        public string InternalTypeFormat
            => "p";

        /// <summary>
        ///     reference kind
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.ConstantValue;

        /// <summary>
        ///     type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.ProcedureType;

        /// <summary>
        ///     no intrinsic routine
        /// </summary>
        public IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Unknown;

        /// <summary>
        ///     add a parameter group
        /// </summary>
        /// <param name="resultType">result type</param>
        /// <param name="kind">procedure kind</param>
        public Routine AddParameterGroup(RoutineKind kind, ITypeReference resultType) {
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
        public Routine AddParameterGroup(string parameterName, RoutineKind kind, ITypeReference firstParam, ITypeReference resultType) {
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

                if (!paramGroup.Matches(TypeRegistry, signature))
                    continue;

                callableRoutines.Add(paramGroup);
            }
        }

    }
}
