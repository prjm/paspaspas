using System.Collections.Generic;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>hi</c> function
    /// </summary>
    public class Hi : IntrinsicRoutine {

        /// <summary>
        ///     name of the function
        /// </summary>
        public override string Name
            => "Hi";

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public override void ResolveCall(IList<ParameterGroup> callableRoutines, Signature signature) {
            if (signature.Length != 1)
                return;

            var param = TypeRegistry.GetTypeByIdOrUndefinedType(signature[0].TypeId);
            if (!param.TypeKind.IsIntegral())
                return;

            var result = new ParameterGroup();
            result.AddParameter("AValue").SymbolType = signature[0];

            if (signature[0].IsConstant)
                result.ResultType = TypeRegistry.Runtime.Integers.Hi(signature[0]);
            else
                result.ResultType = TypeRegistry.Runtime.Types.MakeReference(KnownTypeIds.ByteType);

            callableRoutines.Add(result);

        }
    }
}
