#nullable disable
using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     logical operators: type deduction and constant propagation
    /// </summary>
    public class LogicalOperator : OperatorBase {

        private static void Register(ITypeRegistry registry, OperatorKind kind, int arity = 2)
            => registry.RegisterOperator(new LogicalOperator(kind, arity));

        /// <summary>
        ///     register logical operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, OperatorKind.NotOperator, 1);
            Register(registry, OperatorKind.AndOperator);
            Register(registry, OperatorKind.XorOperator);
            Register(registry, OperatorKind.OrOperator);
            Register(registry, OperatorKind.ShlOperator);
            Register(registry, OperatorKind.ShrOperator);
        }

        /// <summary>
        ///     create a new logical operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        public LogicalOperator(OperatorKind withKind, int withArity)
            : base(withKind, withArity) { }

        /// <summary>
        ///     operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case OperatorKind.AndOperator:
                        return KnownNames.And;
                    case OperatorKind.OrOperator:
                        return KnownNames.Or;
                    case OperatorKind.XorOperator:
                        return KnownNames.Xor;
                    case OperatorKind.NotOperator:
                        return KnownNames.Not;
                    case OperatorKind.ShlOperator:
                        return KnownNames.Shl;
                    case OperatorKind.ShrOperator:
                        return KnownNames.Shr;
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     evaluate an unary operator
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateUnaryOperator(ISignature input, IUnitType currentUnit) {
            var operand = input[0];
            var operations = Runtime.GetLogicalOperators(TypeRegistry, operand);

            if (operations == null)
                return Invalid;

            if (Kind == OperatorKind.NotOperator)
                if (operand.IsConstant(out var constOperand))
                    return operations.NotOperator(constOperand);
                else
                    return operand;

            return Invalid;
        }

        /// <summary>
        ///     binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentUnit"></param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateBinaryOperator(ISignature input, IUnitType currentUnit) {
            var left = input[0];
            var right = input[1];

            if (Kind == OperatorKind.ShrOperator)
                return EvaluateShiftOperator(toLeft: false, left, right);

            if (Kind == OperatorKind.ShlOperator)
                return EvaluateShiftOperator(toLeft: true, left, right);

            var operations = Runtime.GetLogicalOperators(TypeRegistry, left, right);

            if (operations == null)
                return Invalid;

            if (Kind == OperatorKind.AndOperator)
                return EvaluateAndOperator(left, right, operations);

            if (Kind == OperatorKind.OrOperator)
                return EvaluateOrOperator(left, right, operations);

            if (Kind == OperatorKind.XorOperator)
                return EvaluateXorOperator(left, right, operations);

            return Invalid;
        }

        private ITypeSymbol EvaluateShiftOperator(bool toLeft, ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant(out var leftConstant) && right.IsConstant(out var rightConstant))
                if (toLeft)
                    return Runtime.Integers.Shl(leftConstant, rightConstant);
                else
                    return Runtime.Integers.Shr(leftConstant, rightConstant);

            var baseType = left.TypeDefinition;

            if (left.TypeDefinition.IsSubrangeType(out var subrangeType))
                baseType = subrangeType.SubrangeOfType;

            if (!(baseType is IIntegralType intType))
                return Invalid;

            if (right.IsConstant()) {
                if (right is IIntegerValue intValue && intValue.SignedValue >= 0) {
                    if (intValue.SignedValue <= 32 && intType.TypeSizeInBytes < 4)
                        return SystemUnit.IntegerType.Reference;
                    if (intValue.SignedValue <= 32 && intType.TypeSizeInBytes == 4)
                        return baseType.Reference;
                    if (intType.TypeSizeInBytes == 8)
                        return baseType.Reference;
                }

                return Invalid;
            }

            if (intType.TypeSizeInBytes < 4)
                return SystemUnit.IntegerType.Reference;

            return intType.Reference;
        }

        private ITypeSymbol EvaluateXorOperator(ITypeSymbol left, ITypeSymbol right, ILogicalOperations operations) {
            if (left.IsConstant(out var leftConstant) && right.IsConstant(out var rightConstant))
                return operations.XorOperator(leftConstant, rightConstant);
            else
                return GetSmallestBoolOrIntegralType(left, right, 1).Reference;
        }

        private ITypeSymbol EvaluateOrOperator(ITypeSymbol left, ITypeSymbol right, ILogicalOperations operations) {
            if (left.IsConstant(out var leftConstant) && right.IsConstant(out var rightConstant))
                return operations.OrOperator(leftConstant, rightConstant);
            else if (left.IsConstant() && Runtime.Booleans.TrueValue.Equals(left))
                return Runtime.Booleans.TrueValue;
            else if (left.IsConstant() && Runtime.Booleans.FalseValue.Equals(left))
                return right;
            else
                return GetSmallestBoolOrIntegralType(left, right, 1).Reference;
        }

        private ITypeSymbol EvaluateAndOperator(ITypeSymbol left, ITypeSymbol right, ILogicalOperations operations) {
            if (left.IsConstant(out var leftConstant) && right.IsConstant(out var rightConstant))
                return operations.AndOperator(leftConstant, rightConstant);
            else if (left.IsConstant() && Runtime.Booleans.FalseValue.Equals(left))
                return Runtime.Booleans.FalseValue;
            else if (left.IsConstant() && Runtime.Booleans.TrueValue.Equals(left))
                return right;
            else
                return GetSmallestBoolOrIntegralType(left, right, 1).Reference;
        }
    }
}