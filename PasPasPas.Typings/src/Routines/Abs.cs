using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     type specification for the <code>Abs</code> routine
    /// </summary>
    public class Abs : IntrinsicRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "Abs";

        /// <summary>
        ///     try to resolve a call
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public override void ResolveCall(IList<ParameterGroup> callableRoutines, Signature signature) {
            if (signature.Length != 1)
                return;

            var param = TypeRegistry.GetTypeByIdOrUndefinedType(signature[0].TypeId);
            if (!param.TypeKind.IsNumerical())
                return;

            var result = new ParameterGroup();
            result.AddParameter("AValue").SymbolType = signature[0];

            if (signature[0].IsConstant() && param.TypeKind.IsIntegral())
                result.ResultType = TypeRegistry.Runtime.Integers.Abs(signature[0]);
            else if (signature[0].IsConstant() && param.TypeKind == CommonTypeKind.RealType)
                result.ResultType = TypeRegistry.Runtime.RealNumbers.Abs(signature[0]);
            else
                result.ResultType = signature[0];

            callableRoutines.Add(result);
        }

    }
}
