using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

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
                var arrayType = TypeRegistry.TypeCreator.CreateDynamicArrayType(baseType.TypeId, false);
                return TypeRegistry.MakeReference(arrayType.TypeId);
            }

            var leftValue = input[0] as IArrayValue;
            var rightValue = input[1] as IArrayValue;
            using (var list = TypeRegistry.ListPools.GetList<ITypeReference>()) {
                list.Item.AddRange(leftValue.Values);
                list.Item.AddRange(rightValue.Values);
                var indexType = TypeRegistry.TypeCreator.CreateSubrangeType(KnownTypeIds.IntegerType, TypeRegistry.Runtime.Integers.Zero, TypeRegistry.Runtime.Integers.ToScaledIntegerValue(list.Item.Count - 1));
                var arrayType = TypeRegistry.TypeCreator.CreateStaticArrayType(baseType.TypeId, indexType.TypeId, false);
                return TypeRegistry.Runtime.Structured.CreateArrayValue(arrayType.TypeId, baseType.TypeId, TypeRegistry.ListPools.GetFixedArray(list));
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
