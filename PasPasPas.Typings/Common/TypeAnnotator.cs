using System;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Operations;
using PasPasPas.Typings.Simple;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     visitor to annotate typs
    /// </summary>
    public class TypeAnnotator :

        IEndVisitor<ConstantValue>,
        IEndVisitor<UnaryOperator>,
        IEndVisitor<BinaryOperator> {

        private readonly IStartEndVisitor visitor;
        private readonly ITypedEnvironment environment;

        /// <summary>
        ///     as common visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor() =>
            visitor;

        /// <summary>
        ///     create a new type annotator
        /// </summary>
        /// <param name="env">typed environment</param>
        public TypeAnnotator(ITypedEnvironment env) {
            visitor = new Visitor(this);
            environment = env;
        }

        /// <summary>
        ///     determine the type of a constant value
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ConstantValue element) {
            if (element.Kind == ConstantValueKind.HexNumber ||
                element.Kind == ConstantValueKind.Integer ||
                element.Kind == ConstantValueKind.QuotedString ||
                element.Kind == ConstantValueKind.RealNumber ||
                element.Kind == ConstantValueKind.True ||
                element.Kind == ConstantValueKind.False) {
                element.TypeInfo = environment.TypeRegistry.GetTypeOrUndef(LiteralValues.GetTypeFor(element.LiteralValue));
            }
        }

        /// <summary>
        ///     annotate binary operators
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(BinaryOperator element) {
            if (element.Kind == ExpressionKind.And) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.AndOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Or) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.OrOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
            else if (element.Kind == ExpressionKind.Xor) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.XorOperation, element.LeftOperand?.TypeInfo, element.RightOperand?.TypeInfo);
            }
        }

        /// <summary>
        ///     determine the type of an unary operator
        /// </summary>
        /// <param name="element">operator to determine the type of</param>
        public void EndVisit(UnaryOperator element) {
            if (element.Kind == ExpressionKind.Not) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.NotOperation, element.Value?.TypeInfo);
            }
        }

        /// <summary>
        ///     gets the type of a given operator
        /// </summary>
        /// <param name="operatorKind"></param>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        private ITypeDefinition GetTypeOfOperator(int operatorKind, ITypeDefinition typeInfo) {
            if (typeInfo == null)
                return null;

            var operation = environment.TypeRegistry.GetOperator(operatorKind);

            if (operation == null)
                return null;

            var signature = new Signature(typeInfo.TypeId);
            var typeId = operation.GetOutputTypeForOperation(signature);
            return environment.TypeRegistry.GetTypeOrUndef(typeId);
        }

        /// <summary>
        ///     gets the type of a given binary operator
        /// </summary>
        /// <param name="operatorKind"></param>
        /// <param name="typeInfo1"></param>
        /// <param name="typeInfo2"></param>
        /// <returns></returns>
        private ITypeDefinition GetTypeOfOperator(int operatorKind, ITypeDefinition typeInfo1, ITypeDefinition typeInfo2) {
            if (typeInfo1 == null)
                return null;

            if (typeInfo2 == null)
                return null;


            var operation = environment.TypeRegistry.GetOperator(operatorKind);

            if (operation == null)
                return null;

            var signature = new Signature(typeInfo1.TypeId, typeInfo2.TypeId);
            var typeId = operation.GetOutputTypeForOperation(signature);
            return environment.TypeRegistry.GetTypeOrUndef(typeId);
        }




    }
}
