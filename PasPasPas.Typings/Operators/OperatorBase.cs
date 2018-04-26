using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     base class for operators
    /// </summary>
    public abstract class OperatorBase : IOperator {

        private readonly int kind;
        private readonly int arity;

        /// <summary>
        ///     create a new operator definition
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        public OperatorBase(int withKind, int withArity) {
            kind = withKind;
            arity = withArity;
        }

        /// <summary>
        ///     resolve type aliases
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        protected ITypeDefinition ResolveAlias(int typeId) {
            var type = TypeRegistry.GetTypeByIdOrUndefinedType(typeId);
            return TypeBase.ResolveAlias(type);
        }

        /// <summary>
        ///     operator kind
        /// </summary>
        /// <see cref="DefinedOperators"/>
        public int Kind
            => kind;

        /// <summary>
        ///     operator arity (number of operands)
        /// </summary>
        public int Arity
            => arity;

        /// <summary>
        ///     operator name (optional)
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     type registry for type operation
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     runtime values
        /// </summary>
        public IRuntimeValueFactory Runtime { get; set; }

        /// <summary>
        ///     get the output type for an operation
        /// </summary>
        /// <param name="input">input signature</param>
        /// <returns>output type id</returns>
        public ITypeReference EvaluateOperator(Signature input) {
            switch (arity) {
                case 1:
                    return EvaluateUnaryOperator(input);
                case 2:
                    return EvaluateBinaryOperator(input);
            }

            return Runtime.Types.MakeReference(KnownTypeIds.ErrorType);
        }

        /// <summary>
        ///     evaluate unary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ITypeReference EvaluateBinaryOperator(Signature input)
            => Runtime.Types.MakeReference(KnownTypeIds.ErrorType);

        /// <summary>
        ///     evaluate binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ITypeReference EvaluateUnaryOperator(Signature input)
            => Runtime.Types.MakeReference(KnownTypeIds.ErrorType);

        /// <summary>
        ///     helper method: map an expression kind to an registered operator id
        /// </summary>
        /// <param name="kind">expression kind</param>
        /// <param name="leftType">left operand</param>
        /// <param name="rightType">right operand</param>
        /// <returns>operator id</returns>
        public static int GetOperatorId(ExpressionKind kind, CommonTypeKind leftType, CommonTypeKind rightType) {

            if (leftType == CommonTypeKind.UnknownType || rightType == CommonTypeKind.UnknownType)
                return DefinedOperators.Undefined;

            switch (kind) {
                case ExpressionKind.LessThen:
                    return DefinedOperators.LessThen;
                case ExpressionKind.LessThenEquals:
                    return DefinedOperators.LessThenOrEqual;
                case ExpressionKind.GreaterThen:
                    return DefinedOperators.GreaterThen;
                case ExpressionKind.GreaterThenEquals:
                    return DefinedOperators.GreaterThenEqual;
                case ExpressionKind.NotEquals:
                    return DefinedOperators.NotEqualsOperator;
                case ExpressionKind.EqualsSign:
                    return DefinedOperators.EqualsOperator;
                case ExpressionKind.Xor:
                    return DefinedOperators.XorOperation;
                case ExpressionKind.Or:
                    return DefinedOperators.OrOperation;
                case ExpressionKind.Minus:
                    return DefinedOperators.MinusOperation;
                case ExpressionKind.Shr:
                    return DefinedOperators.ShrOperation;
                case ExpressionKind.Shl:
                    return DefinedOperators.ShlOperation;
                case ExpressionKind.And:
                    return DefinedOperators.AndOperation;
                case ExpressionKind.Mod:
                    return DefinedOperators.ModOperation;
                case ExpressionKind.Slash:
                    return DefinedOperators.SlashOperation;
                case ExpressionKind.Times:
                    return DefinedOperators.TimesOperation;
                case ExpressionKind.Div:
                    return DefinedOperators.DivOperation;
                case ExpressionKind.Not:
                    return DefinedOperators.NotOperation;
                case ExpressionKind.UnaryMinus:
                    return DefinedOperators.UnaryMinus;
                case ExpressionKind.UnaryPlus:
                    return DefinedOperators.UnaryPlus;
            };

            if (kind == ExpressionKind.Plus) {
                if (leftType.IsTextual() && rightType.IsTextual())
                    return DefinedOperators.ConcatOperator;

                if (leftType.IsNumerical() && rightType.IsNumerical())
                    return DefinedOperators.PlusOperation;
            }

            return DefinedOperators.Undefined;
        }

        /// <summary>
        ///     get the type kind of a given type reference
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        protected CommonTypeKind GetTypeKind(ITypeReference typeReference)
            => TypeRegistry.GetTypeKind(typeReference.TypeId);

        /// <summary>
        ///     get a reference to the error type
        /// </summary>
        /// <returns></returns>
        protected ITypeReference GetErrorTypeReference()
            => Runtime.Types.MakeReference(KnownTypeIds.ErrorType);

    }
}