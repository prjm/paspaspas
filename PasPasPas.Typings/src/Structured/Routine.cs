﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Serialization;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     class for a routine (procedure or function)
    /// </summary>
    public class Routine : IRoutine {

        /// <summary>
        ///     create a new parameter group
        /// </summary>
        /// <param name="parent">parent routine group</param>
        /// <param name="procedureKind">procedure kind</param>
        public Routine(IRoutineGroup parent, RoutineKind procedureKind) {
            RoutineGroup = parent;
            Kind = procedureKind;
            var errorType = parent.DefiningType.DefiningUnit.TypeRegistry.SystemUnit.ErrorType;
            ResultType = errorType.Reference;
            Parameters = new List<IVariable>();
        }

        /// <summary>
        ///     result type
        /// </summary>
        public ITypeSymbol ResultType { get; set; }

        /// <summary>
        ///     symbols
        /// </summary>
        public IDictionary<string, ITypeSymbol> Symbols { get; }
            = new Dictionary<string, ITypeSymbol>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     routine parameters
        /// </summary>
        public IList<IVariable> Parameters { get; private set; }

        /// <summary>
        ///     class item
        /// </summary>
        public bool IsClassItem { get; set; }

        /// <summary>
        ///     parent routine
        /// </summary>
        public IRoutineGroup RoutineGroup { get; }

        /// <summary>
        ///     code
        /// </summary>
        public ImmutableArray<OpCode> Code { get; set; }

        /// <summary>
        ///     routine kind
        /// </summary>
        public RoutineKind Kind { get; }

        /// <summary>
        ///     routine flags
        /// </summary>
        public RoutineFlags Flags { get; set; }

        /// <summary>
        ///     add a parameter definition
        /// </summary>
        /// <param name="completeName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IVariable AddParameter(string completeName, ITypeSymbol type) {
            if (Parameters == null)
                Parameters = new List<IVariable>();

            var result = new Variable {
                Name = completeName,
                TypeDefinition = type?.TypeDefinition ?? RoutineGroup.DefiningType.DefiningUnit.TypeRegistry.SystemUnit.ErrorType,
            };

            Parameters.Add(result);
            return result;
        }

        /// <summary>
        ///     get a parameter by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IVariable this[int index]
            => Parameters[index];

        /// <summary>
        ///     check if this parameter group matches a signature
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="typeRegistry">type registry</param>
        /// <returns></returns>
        public bool Matches(ITypeRegistry typeRegistry, ISignature signature) {
            var paramCount = Parameters == null ? 0 : Parameters.Count;

            if (paramCount != signature.Count)
                return false;

            var match = true;

            for (var i = 0; Parameters != null && i < Parameters.Count; i++) {
                var parameter = Parameters[i];
                var sourceType = signature[i].TypeDefinition;
                //match = match && typeRegistry.GetTypeByIdOrUndefinedType(parameter.SymbolType.TypeId).CanBeAssignedFrom(sourceType);

                if (!match)
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     create the signature for this parameters
        /// </summary>
        /// <returns></returns>
        public ISignature CreateSignature(ITypeRegistry runtime) {
            if (Parameters == default || Parameters.Count < 1)
                return runtime.Runtime.Types.MakeSignature(ResultType);

            using (var list = runtime.ListPools.GetList<ITypeSymbol>()) {
                var values = new ITypeSymbol[Parameters.Count];
                //for (var i = 0; i < Parameters.Count; i++)
                //   values[i] = Parameters[i].SymbolType ?? runtime.Runtime.Types.MakeErrorTypeReference();

                return runtime.Runtime.Types.MakeSignature(ResultType, runtime.ListPools.GetFixedArray(list));
            }
        }

        /// <summary>
        ///     encode the routine
        /// </summary>
        /// <returns></returns>
        public ImmutableArray<byte> Encode() {
            var result = new byte[2];
            result[0] = Kind.ToByte();
            result[1] = RoutineGroup.RoutineId.ToByte();
            return ImmutableArray.Create<byte>(result);
        }
    }
}