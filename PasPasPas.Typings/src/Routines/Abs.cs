﻿using System.Collections.Generic;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     type specification for the <code>Abs</code> routine
    /// </summary>
    public class Abs : IRoutine {

        /// <summary>
        ///     create a new type specification for the routine <c>abs</c>
        /// </summary>
        /// <param name="registry"></param>
        public Abs(ITypeRegistry registry)
            => TypeRegistry = registry;

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => 0;

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; private set; }

        /// <summary>
        ///     routine name
        /// </summary>
        public string Name
            => "Abs";

        /// <summary>
        ///     constant intrinsic function
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     try to resolve a call
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public void ResolveCall(IList<ParameterGroup> callableRoutines, Signature signature) {
            if (signature.Length != 1)
                return;

            var param = TypeRegistry.GetTypeByIdOrUndefinedType(signature[0].TypeId);
            if (param.TypeKind.IsNumerical()) {
                var result = new ParameterGroup();
                result.AddParameter("AValue").SymbolType = signature[0].TypeId;
                result.ResultType = signature[0].TypeId;
                callableRoutines.Add(result);
            }
        }

    }
}