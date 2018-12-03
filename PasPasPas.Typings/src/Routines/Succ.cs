using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     increment values
    /// </summary>
    public class Succ : IntrinsicRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "Succ";

        private ITypeReference ExecuteCall(ITypeReference value) {

            var typeKind = value.TypeKind;
            var type = TypeRegistry.GetTypeByIdOrUndefinedType(value.TypeId) as IOrdinalType;

            if (typeKind.IsIntegral())
                return TypeRegistry.Runtime.Integers.Add(value, TypeRegistry.Runtime.Integers.One);

            else if (typeKind == CommonTypeKind.AnsiCharType && value is ICharValue charValue)
                return TypeRegistry.Runtime.Chars.ToAnsiCharValue((byte)(0xff & (1u + charValue.AsWideChar)));

            else if (typeKind == CommonTypeKind.WideCharType && value is ICharValue wideCharValue)
                return TypeRegistry.Runtime.Chars.ToWideCharValue((char)(0xffff & (1u + wideCharValue.AsWideChar)));

            else if (typeKind == CommonTypeKind.BooleanType && value is IBooleanValue boolValue)
                return TypeRegistry.Runtime.Booleans.Booleans.ToBoolean(type.BitSize, (1u + boolValue.AsUint));

            else if (typeKind == CommonTypeKind.EnumerationType && value is IEnumeratedValue enumValue)
                return TypeRegistry.Runtime.Types.MakeEnumValue(value.TypeId, ExecuteCall(enumValue.Value));

            return TypeRegistry.Runtime.Types.MakeErrorTypeReference();
        }

        /// <summary>
        ///     increment a value
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public override void ResolveCall(IList<ParameterGroup> callableRoutines, Signature signature) {
            if (signature.Length != 1)
                return;

            var parameter = signature[0];

            var param = TypeRegistry.GetTypeByIdOrUndefinedType(parameter.TypeId);
            if (!param.TypeKind.IsOrdinal())
                return;

            var result = new ParameterGroup();
            result.AddParameter("AValue").SymbolType = parameter;

            if (signature[0].IsConstant())
                result.ResultType = ExecuteCall(parameter);
            else
                result.ResultType = parameter;

            callableRoutines.Add(result);
        }
    }
}
