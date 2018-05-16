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
    public class High : IRoutine {

        /// <summary>
        ///     create a new type specification for the routine <c>high</c>
        /// </summary>
        /// <param name="registry"></param>
        public High(ITypeRegistry registry) {
            TypeRegistry = registry;
        }

        /// <summary>
        ///     routine name
        /// </summary>
        public string Name
            => "High";

        /// <summary>
        ///     constant intrinsinc routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     integer parser
        /// </summary>
        public IRuntimeValueFactory ConstOps { get; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => 0;

        /// <summary>
        ///     try to resolve a call
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public void ResolveCall(IList<ParameterGroup> callableRoutines, Signature signature) {
            if (signature.Length != 1)
                return;

            if (ConstOps == null)
                return;

            var param = TypeRegistry.GetTypeByIdOrUndefinedType(signature[0].TypeId);
            if (param.TypeKind.IsOrdinal()) {
                var ordinalType = param as IOrdinalType;
                var highValue = ConstOps.Integers.ToScaledIntegerValue(ordinalType.HighestElement);
                var typeId = LiteralValues.GetTypeFor(highValue);
                var result = new ParameterGroup();
                result.AddParameter("AValue").SymbolType = signature[0].TypeId;
                result.ResultType = typeId;
                callableRoutines.Add(result);
            }
        }
    }
}
