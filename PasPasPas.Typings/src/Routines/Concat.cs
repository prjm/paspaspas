using System.Collections.Generic;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>concat</c> routine
    /// </summary>
    public class Concat : IntrinsicRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "Concat";

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public override void ResolveCall(IList<ParameterGroup> callableRoutines, Signature signature) {
            if (signature.Length < 1)
                return;

            var param = default(ITypeDefinition);
            var result = new ParameterGroup();

            if (signature.Length == 1) {
                param = TypeRegistry.GetTypeByIdOrUndefinedType(signature[0].TypeId);
                if (!param.TypeKind.IsTextual())
                    return;

                result.AddParameter("AValue").SymbolType = signature[0];
                result.ResultType = signature[0];
                callableRoutines.Add(result);
                return;

            }

            var useUnicode = false;
            var isConstant = signature.IsConstant;

            for (var i = 0; i < signature.Length; i++) {
                param = TypeRegistry.GetTypeByIdOrUndefinedType(signature[i].TypeId);

                if (!param.TypeKind.IsTextual())
                    return;

                useUnicode |= param.TypeKind.IsUnicodeText();

                if (isConstant) {
                    if (result.ResultType == default)
                        result.ResultType = signature[i];
                    else
                        result.ResultType = TypeRegistry.Runtime.Strings.Concat(result.ResultType, signature[i]);
                }
            }

            if (!isConstant) {

                if (useUnicode)
                    result.ResultType = TypeRegistry.MakeReference(KnownTypeIds.UnicodeStringType);
                else
                    result.ResultType = TypeRegistry.MakeReference(KnownTypeIds.AnsiStringType);
            }

            callableRoutines.Add(result);
        }
    }
}
