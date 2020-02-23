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
        protected override ITypeSymbol EvaluateUnaryOperator(Signature input) {
            var operand = input[0];
            if (operand.IsConstant(out var constant))
                return Runtime.MakePointerValue(constant);

            return default;
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
        protected override ITypeSymbol EvaluateBinaryOperator(Signature input) {
            if (!(input[0] is IArrayValue i0))
                return Invalid;
            if (!(input[1] is IArrayValue i1))
                return Invalid;

            var b0 = i0.BaseTypeDefinition;
            var b1 = i1.BaseTypeDefinition;
            var baseType = TypeRegistry.GetBaseTypeForArrayOrSet(b0, b1);

            if (baseType.TypeDefinition.BaseType == BaseType.Error)
                return baseType;

            if (!input.IsConstant) {
                var arrayType = TypeRegistry.TypeCreator.CreateDynamicArrayType(baseType.TypeDefinition, false);
                return arrayType;
            }

            var leftValue = input[0] as IArrayValue;
            var rightValue = input[1] as IArrayValue;
            using (var list = TypeRegistry.ListPools.GetList<IValue>()) {
                list.Item.AddRange(leftValue.Values);
                list.Item.AddRange(rightValue.Values);
                var indexType = TypeRegistry.TypeCreator.CreateSubrangeType(SystemUnit.IntegerType, TypeRegistry.Runtime.Integers.Zero, TypeRegistry.Runtime.Integers.ToScaledIntegerValue(list.Item.Count - 1));
                var arrayType = TypeRegistry.TypeCreator.CreateStaticArrayType(baseType.TypeDefinition, indexType, false);
                return TypeRegistry.Runtime.Structured.CreateArrayValue(arrayType, baseType.TypeDefinition, TypeRegistry.ListPools.GetFixedArray(list));
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
