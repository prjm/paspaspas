using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     address-of operator
    /// </summary>
    public class AddressOfOperator : OperatorBase {

        /// <summary>
        ///     create a new address-of operator
        /// </summary>
        public AddressOfOperator() : base(DefinedOperators.AtOperator, 1) {
        }

        /// <summary>
        ///     operator name
        /// </summary>
        public override string Name
            => "@";

        /// <summary>
        ///     evaluate this operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override ITypeReference EvaluateUnaryOperator(Signature input) {
            var operand = input[0];
            return Runtime.MakePointerValue(operand);
        }
    }

    /// <summary>
    ///     operator for concatenating arrays
    /// </summary>
    public class ArrayConcatOperator : OperatorBase {

        /// <summary>
        ///     create a new array concatenate operator
        /// </summary>
        public ArrayConcatOperator() : base(DefinedOperators.ConcatArrayOperator, 2) {
        }

        /// <summary>
        ///     operator name
        /// </summary>
        public override string Name
            => "+";

        /// <summary>
        ///     evaluate the binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override ITypeReference EvaluateBinaryOperator(Signature input) {
            if (!input[0].IsArrayValue(out var i0))
                return TypeRegistry.Runtime.Types.MakeErrorTypeReference();
            if (!input[1].IsArrayValue(out var i1))
                return TypeRegistry.Runtime.Types.MakeErrorTypeReference();

            var b0 = TypeRegistry.MakeReference(i0.BaseType);
            var b1 = TypeRegistry.MakeReference(i1.BaseType);
            var baseType = TypeRegistry.GetBaseTypeForArrayOrSet(b0, b1);

            if (baseType.TypeId == KnownTypeIds.ErrorType)
                return baseType;

            if (!input.IsConstant) {
                var typeId = TypeRegistry.RequireUserTypeId();
                var arrayType = new DynamicArrayType(typeId) { BaseTypeId = typeId };
                TypeRegistry.RegisterType(arrayType);
                return TypeRegistry.MakeReference(arrayType.TypeId);
            }

            var leftValue = input[0] as IArrayValue;
            var rightValue = input[1] as IArrayValue;
            using (var list = TypeRegistry.ListPools.GetList<ITypeReference>()) {
                list.Item.AddRange(leftValue.Values);
                list.Item.AddRange(rightValue.Values);
                var typeId = TypeRegistry.RequireUserTypeId();
                var arrayType = new StaticArrayType(typeId, ImmutableArray.Create(KnownTypeIds.IntegerType));
                return TypeRegistry.Runtime.Structured.CreateArrayValue(typeId, baseType.TypeId, TypeRegistry.ListPools.GetFixedArray(list));
            }
        }
    }

    /// <summary>
    ///     other operators
    /// </summary>
    public static class OtherOperator {

        /// <summary>
        ///     register operator
        /// </summary>
        /// <param name="registeredTypes"></param>
        internal static void RegisterOperators(RegisteredTypes registeredTypes) {
            registeredTypes.RegisterOperator(new AddressOfOperator());
            registeredTypes.RegisterOperator(new ArrayConcatOperator());
        }
    }
}
