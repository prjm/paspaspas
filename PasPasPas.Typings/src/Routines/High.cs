using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {


    /// <summary>
    ///     type specification for the <code>High</code> routine
    /// </summary>
    public class High : IntrinsicRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "High";

        /// <summary>
        ///     try to resolve a call
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public override void ResolveCall(IList<ParameterGroup> callableRoutines, Signature signature) {
            if (signature.Length != 1)
                return;

            var param = default(ITypeDefinition);

            if (signature[0].IsType() || signature[0].IsConstant())
                param = TypeRegistry.GetTypeByIdOrUndefinedType(signature[0].TypeId);

            if (param.TypeKind.IsOrdinal()) {
                var ordinalType = param as IOrdinalType;
                var highValue = ordinalType.HighestElement;
                var typeId = LiteralValuesHelper.GetTypeFor(highValue);
                var result = new ParameterGroup();
                result.AddParameter("AValue").SymbolType = signature[0];
                result.ResultType = highValue;
                callableRoutines.Add(result);
            }



        }

    }
}
