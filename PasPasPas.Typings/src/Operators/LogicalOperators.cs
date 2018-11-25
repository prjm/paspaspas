using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     logical operators: type deduction and constant propagation
    /// </summary>
    public class LogicalOperator : OperatorBase {

        private static void Register(ITypeRegistry registry, int kind, int arity = 2)
            => registry.RegisterOperator(new LogicalOperator(kind, arity));

        /// <summary>
        ///     register logical operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, DefinedOperators.NotOperator, 1);
            Register(registry, DefinedOperators.AndOperator);
            Register(registry, DefinedOperators.XorOperator);
            Register(registry, DefinedOperators.OrOperator);
            Register(registry, DefinedOperators.ShlOperator);
            Register(registry, DefinedOperators.ShrOperator);
        }

        /// <summary>
        ///     create a new logical operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        public LogicalOperator(int withKind, int withArity)
            : base(withKind, withArity) { }

        /// <summary>
        ///     operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.AndOperator:
                        return "and";
                    case DefinedOperators.OrOperator:
                        return "or";
                    case DefinedOperators.XorOperator:
                        return "xor";
                    case DefinedOperators.NotOperator:
                        return "not";
                    case DefinedOperators.ShlOperator:
                        return "shl";
                    case DefinedOperators.ShrOperator:
                        return "shr";
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     evaluate an unary operator
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        protected override ITypeReference EvaluateUnaryOperator(Signature input) {
            var operand = input[0];
            var operations = Runtime.GetLogicalOperators(TypeRegistry, operand);

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.NotOperator)
                if (operand.IsConstant())
                    return operations.NotOperator(operand);
                else
                    return operand;

            return GetErrorTypeReference();
        }

        /// <summary>
        ///     binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override ITypeReference EvaluateBinaryOperator(Signature input) {
            var left = input[0];
            var right = input[1];

            if (Kind == DefinedOperators.ShrOperator)
                return EvaluateShiftOperator(toLeft: false, left, right);

            if (Kind == DefinedOperators.ShlOperator)
                return EvaluateShiftOperator(toLeft: true, left, right);

            var operations = Runtime.GetLogicalOperators(TypeRegistry, left, right);

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.AndOperator)
                return EvaluateAndOperator(left, right, operations);

            if (Kind == DefinedOperators.OrOperator)
                return EvaluateOrOperator(left, right, operations);

            if (Kind == DefinedOperators.XorOperator)
                return EvaluateXorOperator(left, right, operations);

            return GetErrorTypeReference();
        }

        private ITypeReference EvaluateShiftOperator(bool toLeft, ITypeReference left, ITypeReference right) {
            if (left.IsConstant() && right.IsConstant())
                if (toLeft)
                    return Runtime.Integers.Shl(left, right);
                else
                    return Runtime.Integers.Shr(left, right);

            var baseType = TypeRegistry.GetTypeByIdOrUndefinedType(left.TypeId);

            if (baseType.TypeKind == CommonTypeKind.SubrangeType)
                baseType = TypeRegistry.GetTypeByIdOrUndefinedType(TypeRegistry.GetBaseTypeOfSubrangeType(baseType.TypeId));

            if (!(baseType is IIntegralType intType))
                return GetErrorTypeReference();

            if (right.IsConstant()) {
                if (right is IIntegerValue intValue && intValue.SignedValue >= 0) {
                    if (intValue.SignedValue <= 32 && intType.BitSize < 32)
                        return Runtime.Types.MakeReference(KnownTypeIds.IntegerType, CommonTypeKind.IntegerType);
                    if (intValue.SignedValue <= 32 && intType.BitSize == 32)
                        return Runtime.Types.MakeReference(baseType.TypeId, baseType.TypeKind);
                    if (intType.BitSize == 64)
                        return Runtime.Types.MakeReference(baseType.TypeId, baseType.TypeKind);

                }
                return GetErrorTypeReference();
            }

            if (intType.BitSize < 32)
                return TypeRegistry.MakeReference(KnownTypeIds.IntegerType);

            return TypeRegistry.MakeReference(intType.TypeId);
        }

        private ITypeReference EvaluateXorOperator(ITypeReference left, ITypeReference right, ILogicalOperations operations) {
            if (left.IsConstant() && right.IsConstant())
                return operations.XorOperator(left, right);
            else
                return GetSmallestBoolOrIntegralType(left, right, 1);
        }

        private ITypeReference EvaluateOrOperator(ITypeReference left, ITypeReference right, ILogicalOperations operations) {
            if (left.IsConstant() && right.IsConstant())
                return operations.OrOperator(left, right);
            else if (left.IsConstant() && Runtime.Booleans.TrueValue.Equals(left))
                return Runtime.Booleans.TrueValue;
            else if (left.IsConstant() && Runtime.Booleans.FalseValue.Equals(false))
                return right;
            else
                return GetSmallestBoolOrIntegralType(left, right, 1);
        }

        private ITypeReference EvaluateAndOperator(ITypeReference left, ITypeReference right, ILogicalOperations operations) {
            if (left.IsConstant() && right.IsConstant())
                return operations.AndOperator(left, right);
            else if (left.IsConstant() && Runtime.Booleans.FalseValue.Equals(left))
                return Runtime.Booleans.FalseValue;
            else if (left.IsConstant() && Runtime.Booleans.TrueValue.Equals(left))
                return right;
            else
                return GetSmallestBoolOrIntegralType(left, right, 1);
        }
    }
}