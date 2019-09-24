﻿using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     callable routine
    /// </summary>
    public class Routine : IRoutine, ITypeReference {

        /// <summary>
        ///     create a new routine
        /// </summary>
        /// <param name="name">routine name</param>
        /// <param name="kind">routine kind</param>
        /// <param name="types">type registry</param>
        /// <param name="genericTypeId">generic type id</param>
        public Routine(ITypeRegistry types, string name, ProcedureKind kind, int genericTypeId = KnownTypeIds.ErrorType) {
            Name = name;
            Kind = kind;
            TypeRegistry = types;
            TypeId = genericTypeId;
        }

        /// <summary>
        ///     routine parameters
        /// </summary>
        public IList<ParameterGroup> Parameters { get; }
            = new List<ParameterGroup>();

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
        public ParameterGroup AddParameterGroup() {
            var result = new ParameterGroup();
            Parameters.Add(result);
            return result;
        }

        /// <summary>
        ///     add a parameter group
        /// </summary>
        /// <param name="resultType">result type</param>
        public ParameterGroup AddParameterGroup(ITypeReference resultType) {
            var result = new ParameterGroup {
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
        /// <returns></returns>
        public ParameterGroup AddParameterGroup(string parameterName, ITypeReference firstParam, ITypeReference resultType) {
            var result = new ParameterGroup {
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
