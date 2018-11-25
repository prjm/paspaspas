using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     type specification for the <c>chr</c> routine
    /// </summary>
    public class Chr : IntrinsicRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "Chr";

        /// <summary>
        ///     resolve call
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

            if (signature[0].IsConstant())
                result.ResultType = TypeRegistry.Runtime.Integers.Chr(signature[0]);
            else
                result.ResultType = TypeRegistry.Runtime.Types.MakeReference(KnownTypeIds.CharType, TypeRegistry.GetTypeKindOf(KnownTypeIds.CharType));

            callableRoutines.Add(result);
        }

    }
}