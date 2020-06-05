#nullable disable
using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Other;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     boolean value factory and operations on boolean values
    /// </summary>
    public class BooleanOperations : IBooleanOperations {

        /// <summary>
        ///     create a new boolean operation support class
        /// </summary>
        /// <param name="typeRegistryProvider"></param>
        public BooleanOperations(ITypeRegistryProvider typeRegistryProvider) {
            provider = typeRegistryProvider;
            trueValue = new Lazy<IBooleanValue>(() => new BooleanValue(true, provider.GetBooleanType()), true);
            falseValue = new Lazy<IBooleanValue>(() => new BooleanValue(true, provider.GetBooleanType()), true);
            invalid = new Lazy<IValue>(() => new ErrorValue(provider.GetErrorType(), SpecialConstantKind.InvalidBool));
        }

        private readonly ITypeRegistryProvider provider;
        private readonly Lazy<IBooleanValue> trueValue;
        private readonly Lazy<IBooleanValue> falseValue;
        private readonly Lazy<IValue> invalid;

        /// <summary>
        ///     constant value: invalid boolean value
        /// </summary>
        public IValue Invalid
            => invalid.Value;

        /// <summary>
        ///     constant value: <c>true</c>
        /// </summary>
        public IBooleanValue TrueValue
            => trueValue.Value;

        /// <summary>
        ///     constant value: <c>false</c>
        /// </summary>
        public IBooleanValue FalseValue
            => falseValue.Value;

        /// <summary>
        ///     boolean operations
        /// </summary>
        public IBooleanOperations Booleans
            => this;

        /// <summary>
        ///     convert a simple boolean to a boolean runtime value
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="typeDef">type definition</param>
        /// <returns>boolean constant value</returns>
        public IBooleanValue ToBoolean(bool value, ITypeDefinition typeDef) {
            if (typeDef == provider.GetBooleanType())
                return value ? TrueValue : FalseValue;

            if (!(typeDef is IBooleanType boolean))
                throw new ArgumentException(string.Empty, nameof(typeDef));

            switch (boolean.Kind) {
                case BooleanTypeKind.Boolean:
                    return new BooleanValue(value, typeDef);

                case BooleanTypeKind.ByteBool:
                    return new ByteBooleanValue(value ? byte.MaxValue : (byte)0, typeDef);

                case BooleanTypeKind.WordBool:
                    return new LongBooleanValue(value ? ushort.MaxValue : (ushort)0, typeDef);

                case BooleanTypeKind.LongBool:
                    return new LongBooleanValue(value ? uint.MaxValue : 0, typeDef);

                default:
                    throw new ArgumentException(string.Empty, nameof(typeDef));
            }
        }

        private ITypeDefinition EnlargeType(IBooleanValue t1, IBooleanValue t2)
            => t2.TypeDefinition.TypeSizeInBytes > t1.TypeDefinition.TypeSizeInBytes ?
            t2.TypeDefinition :
            t1.TypeDefinition;

        /// <summary>
        ///     <c>and</c> operation
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public IValue AndOperator(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IBooleanValue boolean1 && secondOperand is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.And(boolean1, boolean2), EnlargeType(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>==</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue Equal(IValue left, IValue right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.Equal(boolean1, boolean2), EnlargeType(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue GreaterThen(IValue left, IValue right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.GreaterThen(boolean1, boolean2), EnlargeType(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue GreaterThenEqual(IValue left, IValue right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.GreaterThenEqual(boolean1, boolean2), EnlargeType(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue LessThen(IValue left, IValue right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.LessThen(boolean1, boolean2), EnlargeType(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue LessThenOrEqual(IValue left, IValue right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.LessThenOrEqual(boolean1, boolean2), EnlargeType(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>not</c> operation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValue NotOperator(IValue value) {
            if (value is IBooleanValue boolean)
                return ToBoolean(BooleanValueBase.Not(boolean), value.TypeDefinition);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;&lt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue NotEquals(IValue left, IValue right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.NotEquals(boolean1, boolean2), EnlargeType(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///    <c>or</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue OrOperator(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.Or(boolean1, boolean2), EnlargeType(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>xor</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue XorOperator(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.Xor(boolean1, boolean2), EnlargeType(boolean1, boolean2));
            else
                return Invalid;

        }

        /// <summary>
        ///     create a standard boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IBooleanValue ToBoolean(bool value)
            => ToBoolean(value, provider.GetBooleanType());

        /// <summary>
        ///
        /// </summary>
        /// <param name="unsignedValue"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        public IBooleanValue ToByteBool(byte unsignedValue, ITypeDefinition typeDef)
            => new ByteBooleanValue(unsignedValue, typeDef);

        /// <summary>
        ///
        /// </summary>
        /// <param name="unsignedValue"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        public IBooleanValue ToWordBool(ushort unsignedValue, ITypeDefinition typeDef)
            => new WordBooleanValue(unsignedValue, typeDef);

        /// <summary>
        ///
        /// </summary>
        /// <param name="unsignedValue"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        public IBooleanValue ToLongBool(uint unsignedValue, ITypeDefinition typeDef)
            => new LongBooleanValue(unsignedValue, typeDef);

        /// <summary>
        ///     to boolean value
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IBooleanValue ToBoolean(IBooleanType typeDef, uint value) {
            switch (typeDef.Kind) {

                case BooleanTypeKind.Boolean:
                    return ToBoolean(value != 0u);

                case BooleanTypeKind.ByteBool:
                    return ToByteBool((byte)value, typeDef);

                case BooleanTypeKind.WordBool:
                    return ToWordBool((ushort)value, typeDef);

                case BooleanTypeKind.LongBool:
                    return ToLongBool(value, typeDef);
            }

            throw new ArgumentOutOfRangeException(nameof(typeDef));
        }
    }
}
