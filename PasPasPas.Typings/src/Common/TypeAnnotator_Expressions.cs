using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Operators;

namespace PasPasPas.Typings.Common {

    public partial class TypeAnnotator {


        /// <summary>
        ///     determine the type of a constant value
        /// </summary>
        /// <param name="element">constant value</param>
        public void EndVisit(ConstantValue element) {

            // some constant literals have already assigned type information
            // nothing has to be done
            if (element.TypeInfo != default)
                return;

            if (element.Kind == ConstantValueKind.True) {
                element.TypeInfo = Runtime.Booleans.TrueValue;
                return;
            }

            if (element.Kind == ConstantValueKind.False) {
                element.TypeInfo = Runtime.Booleans.FalseValue;
                return;
            }

            if (element.Kind == ConstantValueKind.Nil) {
                element.TypeInfo = Runtime.Types.Nil;
                return;
            }

            element.TypeInfo = GetErrorTypeReference(element);
        }

        /// <summary>
        ///     annotate types for binary operators
        /// </summary>
        /// <param name="element">operator to annotate</param>
        public void StartVisit(BinaryOperator element) {
            if (element.LeftOperand is IRequiresArrayExpression e1)
                e1.RequiresArray = element.RequiresArray;

            if (element.RightOperand is IRequiresArrayExpression e2)
                e2.RequiresArray = element.RequiresArray;
        }

        /// <summary>
        ///     annotate types for binary operators
        /// </summary>
        /// <param name="element">operator to annotate</param>
        public void EndVisit(BinaryOperator element) {
            var left = GetTypeRefence(element.LeftOperand);
            var right = GetTypeRefence(element.RightOperand);

            // special case range operator: the range operator is
            // part of a type definition and references types, not values
            if (element.Kind == ExpressionKind.RangeOperator) {
                var resultType = TypeRegistry.GetTypeForSubrangeType(left, right);
                element.TypeInfo = TypeRegistry.MakeTypeReference(resultType);
                return;
            }

            var operatorId = TypeRegistry.GetOperatorId(element.Kind, left, right);
            var binaryOperator = TypeRegistry.GetOperator(operatorId);
            if (operatorId == DefinedOperators.Undefined || binaryOperator == null) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            element.TypeInfo = binaryOperator.EvaluateOperator(new Signature(left, right));
        }

        /// <summary>
        ///     determine the type of an unary operator
        /// </summary>
        /// <param name="element">operator to determine the type of</param>
        public void EndVisit(UnaryOperator element) {

            var operand = element.Value;

            if (operand == null || operand.TypeInfo == null) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            if (element.Kind == ExpressionKind.Not) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.NotOperator, GetTypeRefence(operand));
                return;
            }

            if (element.Kind == ExpressionKind.UnaryMinus) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.UnaryMinus, GetTypeRefence(operand));
                return;
            }

            if (element.Kind == ExpressionKind.UnaryPlus) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.UnaryPlus, GetTypeRefence(operand));
                return;
            }

            if (element.Kind == ExpressionKind.AddressOf) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.AtOperator, GetTypeRefence(operand));
                return;
            }

            element.TypeInfo = GetErrorTypeReference(element);
        }

        /// <summary>
        ///     gets the type of a given unary operator
        /// </summary>
        /// <param name="operatorKind"></param>
        /// <param name="operand"></param>
        /// <returns></returns>
        private ITypeReference GetTypeOfOperator(int operatorKind, ITypeReference operand) {
            if (operand == null)
                return GetErrorTypeReference(null);

            var unaryOperator = TypeRegistry.GetOperator(operatorKind);

            if (unaryOperator == null)
                return GetErrorTypeReference(null);

            return unaryOperator.EvaluateOperator(new Signature(operand));
        }

    }
}
