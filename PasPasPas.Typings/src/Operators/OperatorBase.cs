using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     common base class for operators
    /// </summary>
    public abstract class OperatorBase : IOperator {

        /// <summary>
        ///     create a new operator definition
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        protected OperatorBase(int withKind, int withArity) {
            Kind = withKind;
            Arity = withArity;
            invalidResult = new Lazy<IValue>(() => TypeRegistry.Runtime.Types.MakeInvalidValue(SpecialConstantKind.InvalidResult));
        }

        /// <summary>
        ///     operator kind
        /// </summary>
        /// <see cref="DefinedOperators"/>
        public int Kind { get; }

        /// <summary>
        ///     operator arity (number of operands)
        /// </summary>
        public int Arity { get; }

        private readonly Lazy<IValue> invalidResult;

        /// <summary>
        ///     system unit
        /// </summary>
        protected ISystemUnit SystemUnit
            => TypeRegistry.SystemUnit;

        /// <summary>
        ///     invalid value
        /// </summary>
        public IValue Invalid
            => invalidResult.Value;

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
        public IRuntimeValueFactory Runtime
            => TypeRegistry.Runtime;

        /// <summary>
        ///     get the output type for an operation
        /// </summary>
        /// <param name="input">input signature</param>
        /// <returns>output type id</returns>
        public ITypeSymbol EvaluateOperator(ISignature input) {

            if (input.ParameterCount != Arity)
                throw new InvalidOperationException();

            switch (Arity) {
                case 1:
                    return EvaluateUnaryOperator(input);
                case 2:
                    return EvaluateBinaryOperator(input);
            }

            return Invalid;
        }

        /// <summary>
        ///     evaluate binary operator
        /// </summary>
        /// <param name="input">input parameters</param>
        /// <returns>operator result</returns>
        protected virtual ITypeSymbol EvaluateBinaryOperator(ISignature input)
            => Invalid;

        /// <summary>
        ///     evaluate binary operator
        /// </summary>
        /// <param name="input">operator parameters</param>
        /// <returns></returns>
        protected virtual ITypeSymbol EvaluateUnaryOperator(ISignature input)
            => Invalid;

        /// <summary>
        ///     create a type reference to the smallest integral type for two operands
        /// </summary>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <param name="minBitSize">minimal operator size</param>
        /// <returns>type reference</returns>
        protected ITypeDefinition GetSmallestIntegralType(ITypeSymbol left, ITypeSymbol right, int minBitSize)
            => TypeRegistry.GetSmallestIntegralTypeOrNext(left.TypeDefinition, right.TypeDefinition, minBitSize);

        /// <summary>
        ///     get the smallest boolean type for a minimum bit size
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="minBisSize"></param>
        /// <returns></returns>
        protected ITypeDefinition GetSmallestBooleanType(ITypeSymbol left, ITypeSymbol right, int minBisSize)
            => TypeRegistry.GetSmallestBooleanTypeOrNext(left.TypeDefinition, right.TypeDefinition, minBisSize);

        /// <summary>
        ///     create a type reference to the smallest integral type for two operands
        /// </summary>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <param name="minBitSize">minimal number of required bits</param>
        /// <returns>type reference</returns>
        protected ITypeDefinition GetSmallestRealOrIntegralType(ITypeSymbol left, ITypeSymbol right, int minBitSize) {
            if (left.TypeDefinition.BaseType == BaseType.Real || right.TypeDefinition.BaseType == BaseType.Real)
                return ExtendedType;

            return TypeRegistry.GetSmallestIntegralTypeOrNext(left.TypeDefinition, right.TypeDefinition, minBitSize);
        }

        /// <summary>
        ///     create a type reference to the smallest boolean or integral type for two operands
        /// </summary>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <param name="minBitSize">minimal number of required bits</param>
        /// <returns>type reference</returns>
        protected ITypeDefinition GetSmallestBoolOrIntegralType(ITypeSymbol left, ITypeSymbol right, int minBitSize) {

            if (left.TypeDefinition.IsSubrangeType(out var subrangeType1))
                return GetSmallestBoolOrIntegralType(subrangeType1.SubrangeOfType, right, minBitSize);

            if (left.TypeDefinition.IsSubrangeType(out var subrangeType2))
                return GetSmallestBoolOrIntegralType(left, subrangeType2.SubrangeOfType, minBitSize);

            if (left.TypeDefinition.BaseType == BaseType.Boolean && right.TypeDefinition.BaseType == BaseType.Boolean)
                return GetSmallestBooleanType(left, right, minBitSize);

            return GetSmallestIntegralType(left, right, minBitSize);
        }


        /// <summary>
        ///     get a reference to the extended type
        /// </summary>
        /// <returns>type reference</returns>
        protected ITypeDefinition ExtendedType
            => TypeRegistry.SystemUnit.ExtendedType;


    }
}