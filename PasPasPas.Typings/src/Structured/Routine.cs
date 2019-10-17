﻿using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     callable routine
    /// </summary>
    public class Routine : IRoutine, ITypeReference {

        /// <summary>
        ///     create a new routine
        /// </summary>
        /// <param name="name">routine name</param
        /// <param name="types">type registry</param>
        /// <param name="definingType">defining type</param>
        /// <param name="genericTypeId">generic type id</param>
        public Routine(ITypeRegistry types, string name, int genericTypeId = KnownTypeIds.ErrorType, int definingType = KnownTypeIds.ErrorType) {
            Name = name;
            TypeRegistry = types;
            TypeId = genericTypeId;
            DefiningType = definingType;
        }

        /// <summary>
        ///     routine parameters
        /// </summary>
        public IList<IParameterGroup> Parameters { get; }
            = new List<IParameterGroup>();

        /// <summary>
        ///     routine name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind { get; }

        /// <summary>
        ///     used type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; private set; }

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
        ///     add a parameter group
        /// </summary>
        /// <returns></returns>
        /// <param name="kind">procedure kind</param>
        public ParameterGroup AddParameterGroup(ProcedureKind kind) {
            var result = new ParameterGroup(this, kind);
            Parameters.Add(result);
            return result;
        }

        /// <summary>
        ///     add a parameter group
        /// </summary>
        /// <param name="resultType">result type</param>
        /// <param name="kind">procedure kind</param>
        public ParameterGroup AddParameterGroup(ProcedureKind kind, ITypeReference resultType) {
            var result = new ParameterGroup(this, kind) {
                ResultType = resultType
            };

            Parameters.Add(result);
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
        public ParameterGroup AddParameterGroup(string parameterName, ProcedureKind kind, ITypeReference firstParam, ITypeReference resultType) {
            var result = new ParameterGroup(this, kind) {
                ResultType = resultType
            };

            result.AddParameter(parameterName).SymbolType = firstParam;

            Parameters.Add(result);
            return result;
        }

        /// <summary>
        ///     find a matching parameter group
        /// </summary>
        /// <param name="callableRoutines">list of callable routines</param>
        /// <param name="signature">used signature</param>
        public void ResolveCall(IList<IParameterGroup> callableRoutines, Signature signature) {
            foreach (var paramGroup in Parameters) {

                if (!paramGroup.Matches(TypeRegistry, signature))
                    continue;

                callableRoutines.Add(paramGroup);
            }
        }

    }
}
