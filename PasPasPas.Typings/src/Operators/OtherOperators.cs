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
        public AddressOfOperator() : base(OperatorKind.AtOperator, 1) {
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
        /// <param name="currentUnit">current unit</param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateUnaryOperator(ISignature input, IUnitType currentUnit) {
            var operand = input[0];
            if (operand.IsConstant(out var constant))
                return Runtime.MakePointerValue(constant);

            return Invalid;
        }
    }

    /// <summary>
    ///     operator for concatenating arrays
    /// </summary>
    public class ArrayConcatOperator : OperatorBase {

        /// <summary>
        ///     create a new array concatenate operator
        /// </summary>
        public ArrayConcatOperator() : base(OperatorKind.ConcatArrayOperator, 2) {
        }

        /// <summary>
        ///     operator name
        /// </summary>
        public override string Name
            => KnownNames.Plus;

        /// <summary>
        ///     evaluate the binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentUnit">current unit</param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateBinaryOperator(ISignature input, IUnitType currentUnit) {
            if (!(input[0] is IArrayValue i0))
                return Invalid;
            if (!(input[1] is IArrayValue i1))
                return Invalid;

            var b0 = i0.BaseTypeDefinition;
            var b1 = i1.BaseTypeDefinition;
            var baseType = TypeRegistry.GetBaseTypeForArrayOrSet(b0.Reference, b1.Reference);
            var typeCreator = TypeRegistry.CreateTypeFactory(currentUnit);

            if (baseType.BaseType == BaseType.Error)
                return baseType.Reference;

            if (!input.HasConstantParameters) {
                var arrayType = typeCreator.CreateDynamicArrayType(baseType, string.Empty, false);
                return arrayType.Reference;
            }

            using (var list = TypeRegistry.ListPools.GetList<IValue>()) {
                list.Item.AddRange(i0.Values);
                list.Item.AddRange(i1.Values);
                var indexType = typeCreator.CreateSubrangeType(SystemUnit.IntegerType, TypeRegistry.Runtime.Integers.Zero, TypeRegistry.Runtime.Integers.ToScaledIntegerValue(list.Item.Count - 1));
                var arrayType = typeCreator.CreateStaticArrayType(baseType, string.Empty, indexType, false);
                return TypeRegistry.Runtime.Structured.CreateArrayValue(arrayType, baseType, TypeRegistry.ListPools.GetFixedArray(list));
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
