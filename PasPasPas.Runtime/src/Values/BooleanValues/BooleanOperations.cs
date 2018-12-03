using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     boolean value factory and operations
    /// </summary>
    public class BooleanOperations : IBooleanOperations {

        /// <summary>
        ///     constant value: invalid boolean value
        /// </summary>
        public ITypeReference Invalid { get; }
            = new SpecialValue(SpecialConstantKind.InvalidBool);

        /// <summary>
        ///     constant value: <c>true</c>
        /// </summary>
        public IBooleanValue TrueValue { get; }
            = new BooleanValue(true);

        /// <summary>
        ///     constant value: <c>false</c>
        /// </summary>
        public IBooleanValue FalseValue { get; }
            = new BooleanValue(false);

        /// <summary>
        ///     boolean operations
        /// </summary>
        public IBooleanOperations Booleans
            => this;

        /// <summary>
        ///     convert a simple bool to a boolean value
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns>boolean constant value</returns>
        public IBooleanValue ToBoolean(bool value)
            => value ? TrueValue : FalseValue;

        /// <summary>
        ///     <c>and</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference AndOperator(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.And(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>==</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference Equal(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.Equal(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference GreaterThen(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.GreaterThen(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;=</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference GreaterThenEqual(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.GreaterThenEqual(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference LessThen(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.LessThen(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;=</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference LessThenOrEqual(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.LessThenOrEqual(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>not</c> operation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference NotOperator(ITypeReference value) {
            if (value is IBooleanValue boolean)
                return ToBoolean(BooleanValueBase.Not(boolean));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;&lt;</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference NotEquals(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.NotEquals(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///    <c>or</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference OrOperator(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.Or(boolean1, boolean2));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>xor</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference XorOperator(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.Xor(boolean1, boolean2));
            else
                return Invalid;

        }

        /// <summary>
        ///     get a byte bool value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IBooleanValue ToByteBool(byte value)
            => new ByteBooleanValue(value);

        /// <summary>
        ///     get a word bool value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IBooleanValue ToWordBool(ushort value)
            => new WordBooleanValue(value);

        /// <summary>
        ///     get a word bool value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IBooleanValue ToLongBool(uint value)
            => new LongBooleanValue(value);


        /// <summary>
        ///     get a new boolean value depending on the bit size
        /// </summary>
        /// <param name="bitSize"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference ToBoolean(uint bitSize, uint value) {
            switch (bitSize) {
                case 1:
                    return value == 0 ? FalseValue : TrueValue;

                case 8:
                    return ToByteBool((byte)value);

                case 16:
                    return ToWordBool((ushort)value);

                case 32:
                    return ToLongBool(value);

                default:
                    return Invalid;
            }
        }
    }
}
