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
        ///     create a new type specification for the routine <c>abs</c>
        /// </summary>
        /// <param name="registry"></param>
        public Abs(ITypeRegistry registry)
            => TypeRegistry = registry;

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
            if (param.TypeKind.IsNumerical()) {
                var result = new ParameterGroup();
                result.AddParameter("AValue").SymbolType = signature[0];

                if (signature[0].IsConstant) {
                    if (param.TypeKind.IsIntegral())
                        result.ResultType = TypeRegistry.Runtime.Integers.Abs(signature[0]);
                    else if (param.TypeKind == CommonTypeKind.RealType)
                        result.ResultType = TypeRegistry.Runtime.RealNumbers.Abs(signature[0]);
                    else
                        return;
                }
                else {
                    result.ResultType = signature[0];
                }
                callableRoutines.Add(result);
            }
        }

    }
}
