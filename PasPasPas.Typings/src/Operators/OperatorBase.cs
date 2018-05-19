﻿using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
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
        public IRuntimeValueFactory Runtime
            => TypeRegistry?.Runtime;

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

            return GetErrorTypeReference();
        }

        /// <summary>
        ///     evaluate binary operator
        /// </summary>
        /// <param name="input">input parameters</param>
        /// <returns>operator result</returns>
        protected virtual ITypeReference EvaluateBinaryOperator(Signature input)
            => GetErrorTypeReference();

        /// <summary>
        ///     evaluate binary operator
        /// </summary>
        /// <param name="input">operator parameters</param>
        /// <returns></returns>
        protected virtual ITypeReference EvaluateUnaryOperator(Signature input)
            => GetErrorTypeReference();

        /// <summary>
        ///     get a reference to the error type
        /// </summary>
        /// <returns></returns>
        protected ITypeReference GetErrorTypeReference()
            => TypeRegistry.MakeReference(KnownTypeIds.ErrorType);

        /// <summary>
        ///     create a type reference to the smallest integral type for two operands
        /// </summary>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <param name="minBitSize">minimal operator size</param>
        /// <returns>type reference</returns>
        protected ITypeReference GetSmallestIntegralType(ITypeReference left, ITypeReference right, int minBitSize)
            => TypeRegistry.MakeReference(TypeRegistry.GetSmallestIntegralTypeOrNext(left.TypeId, right.TypeId, minBitSize));

        /// <summary>
        ///     create a type reference to the smallest integral type for two operands
        /// </summary>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <param name="minBitSize">minimal number of required bits</param>
        /// <returns>type reference</returns>
        protected ITypeReference GetSmallestRealOrIntegralType(ITypeReference left, ITypeReference right, int minBitSize) {
            if (left.TypeKind == CommonTypeKind.RealType || right.TypeKind == CommonTypeKind.RealType)
                return GetExtendedType();

            return TypeRegistry.MakeReference(TypeRegistry.GetSmallestIntegralTypeOrNext(left.TypeId, right.TypeId, minBitSize));
        }

        /// <summary>
        ///     create a type reference to the smallest boolean or integral type for two operands
        /// </summary>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <param name="minBitSize">minimal number of required bits</param>
        /// <returns>type reference</returns>
        protected ITypeReference GetSmallestBoolOrIntegralType(ITypeReference left, ITypeReference right, int minBitSize) {

            if (left.TypeKind == CommonTypeKind.SubrangeType)
                return GetSmallestBoolOrIntegralType(TypeRegistry.MakeReference(TypeRegistry.GetBaseTypeOfSubrangeType(left.TypeId)), right, minBitSize);

            if (right.TypeKind == CommonTypeKind.SubrangeType)
                return GetSmallestBoolOrIntegralType(left, TypeRegistry.MakeReference(TypeRegistry.GetBaseTypeOfSubrangeType(right.TypeId)), minBitSize);


            if (left.TypeKind == CommonTypeKind.BooleanType && right.TypeKind == CommonTypeKind.BooleanType)
                return TypeRegistry.MakeReference(TypeRegistry.GetSmallestBooleanTypeOrNext(left.TypeId, right.TypeId, minBitSize));

            return TypeRegistry.MakeReference(TypeRegistry.GetSmallestIntegralTypeOrNext(left.TypeId, right.TypeId, minBitSize));
        }


        /// <summary>
        ///     get a reference to the extended type
        /// </summary>
        /// <returns>type reference</returns>
        protected ITypeReference GetExtendedType()
            => TypeRegistry.MakeReference(KnownTypeIds.Extended);

    }
}