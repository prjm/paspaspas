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
        public IOldTypeReference EvaluateOperator(Signature input) {
            switch (Arity) {
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
        protected virtual IOldTypeReference EvaluateBinaryOperator(Signature input)
            => GetErrorTypeReference();

        /// <summary>
        ///     evaluate binary operator
        /// </summary>
        /// <param name="input">operator parameters</param>
        /// <returns></returns>
        protected virtual IOldTypeReference EvaluateUnaryOperator(Signature input)
            => GetErrorTypeReference();

        /// <summary>
        ///     get a reference to the error type
        /// </summary>
        /// <returns></returns>
        protected IOldTypeReference GetErrorTypeReference()
            => TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.ErrorType);

        /// <summary>
        ///     create a type reference to the smallest integral type for two operands
        /// </summary>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <param name="minBitSize">minimal operator size</param>
        /// <returns>type reference</returns>
        protected IOldTypeReference GetSmallestIntegralType(IOldTypeReference left, IOldTypeReference right, int minBitSize)
            => TypeRegistry.MakeTypeInstanceReference(TypeRegistry.GetSmallestIntegralTypeOrNext(left.TypeId, right.TypeId, minBitSize));

        /// <summary>
        ///     create a type reference to the smallest integral type for two operands
        /// </summary>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <param name="minBitSize">minimal number of required bits</param>
        /// <returns>type reference</returns>
        protected IOldTypeReference GetSmallestRealOrIntegralType(IOldTypeReference left, IOldTypeReference right, int minBitSize) {
            if (left.TypeKind == CommonTypeKind.RealType || right.TypeKind == CommonTypeKind.RealType)
                return GetExtendedType();

            return TypeRegistry.MakeTypeInstanceReference(TypeRegistry.GetSmallestIntegralTypeOrNext(left.TypeId, right.TypeId, minBitSize));
        }

        /// <summary>
        ///     create a type reference to the smallest boolean or integral type for two operands
        /// </summary>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <param name="minBitSize">minimal number of required bits</param>
        /// <returns>type reference</returns>
        protected IOldTypeReference GetSmallestBoolOrIntegralType(IOldTypeReference left, IOldTypeReference right, int minBitSize) {

            if (TypeRegistry.IsSubrangeType(left.TypeId, out var subrangeType1))
                return GetSmallestBoolOrIntegralType(TypeRegistry.MakeTypeInstanceReference(subrangeType1.BaseType.TypeId), right, minBitSize);

            if (TypeRegistry.IsSubrangeType(right.TypeId, out var subrangeType2))
                return GetSmallestBoolOrIntegralType(left, TypeRegistry.MakeTypeInstanceReference(subrangeType2.BaseType.TypeId), minBitSize);

            if (left.TypeKind == CommonTypeKind.BooleanType && right.TypeKind == CommonTypeKind.BooleanType)
                return TypeRegistry.MakeTypeInstanceReference(TypeRegistry.GetSmallestBooleanTypeOrNext(left.TypeId, right.TypeId, minBitSize));

            return TypeRegistry.MakeTypeInstanceReference(TypeRegistry.GetSmallestIntegralTypeOrNext(left.TypeId, right.TypeId, minBitSize));
        }


        /// <summary>
        ///     get a reference to the extended type
        /// </summary>
        /// <returns>type reference</returns>
        protected IOldTypeReference GetExtendedType()
            => TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.Extended);

        /// <summary>
        ///     get a referenced type definition by id
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        protected ITypeDefinition GetTypeByIdOrUndefinedType(int typeId)
            => TypeRegistry.GetTypeByIdOrUndefinedType(typeId);

        /// <summary>
        ///     get a reference to the a specified type
        /// </summary>
        /// <returns>type reference</returns>
        protected IOldTypeReference MakeTypeInstanceReference(int typeId)
            => TypeRegistry.MakeTypeInstanceReference(typeId);


    }
}