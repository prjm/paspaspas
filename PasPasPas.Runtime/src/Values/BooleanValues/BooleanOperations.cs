using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     boolean value factory and operations on boolean values
    /// </summary>
    public class BooleanOperations : IBooleanOperations {

        /// <summary>
        ///     constant value: invalid boolean value
        /// </summary>
        public IOldTypeReference Invalid { get; }
            = new SpecialValue(SpecialConstantKind.InvalidBool);

        /// <summary>
        ///     constant value: <c>true</c>
        /// </summary>
        public IBooleanValue TrueValue { get; }
            = new BooleanValue(true, KnownTypeIds.BooleanType);

        /// <summary>
        ///     constant value: <c>false</c>
        /// </summary>
        public IBooleanValue FalseValue { get; }
            = new BooleanValue(false, KnownTypeIds.BooleanType);

        /// <summary>
        ///     boolean operations
        /// </summary>
        public IBooleanOperations Booleans
            => this;

        /// <summary>
        ///     convert a simple bool to a boolean value
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="typeId"></param>
        /// <returns>boolean constant value</returns>
        public IBooleanValue ToBoolean(bool value, int typeId)
            => typeId == KnownTypeIds.BooleanType ? value ? TrueValue : FalseValue : new BooleanValue(value, typeId);

        /// <summary>
        ///     <c>and</c> operation
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public IOldTypeReference AndOperator(IOldTypeReference firstOperand, IOldTypeReference secondOperand) {
            if (firstOperand is IBooleanValue boolean1 && secondOperand is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.And(boolean1, boolean2), KnownTypeIds.BooleanType);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>==</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference Equal(IOldTypeReference left, IOldTypeReference right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.Equal(boolean1, boolean2), KnownTypeIds.BooleanType);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference GreaterThen(IOldTypeReference left, IOldTypeReference right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.GreaterThen(boolean1, boolean2), KnownTypeIds.BooleanType);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference GreaterThenEqual(IOldTypeReference left, IOldTypeReference right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.GreaterThenEqual(boolean1, boolean2), KnownTypeIds.BooleanType);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference LessThen(IOldTypeReference left, IOldTypeReference right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.LessThen(boolean1, boolean2), KnownTypeIds.BooleanType);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference LessThenOrEqual(IOldTypeReference left, IOldTypeReference right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.LessThenOrEqual(boolean1, boolean2), KnownTypeIds.BooleanType);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>not</c> operation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOldTypeReference NotOperator(IOldTypeReference value) {
            if (value is IBooleanValue boolean)
                return ToBoolean(BooleanValueBase.Not(boolean), KnownTypeIds.BooleanType);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;&lt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference NotEquals(IOldTypeReference left, IOldTypeReference right) {
            if (left is IBooleanValue boolean1 && right is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.NotEquals(boolean1, boolean2), KnownTypeIds.BooleanType);
            else
                return Invalid;
        }

        /// <summary>
        ///    <c>or</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IOldTypeReference OrOperator(IOldTypeReference value1, IOldTypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.Or(boolean1, boolean2), KnownTypeIds.BooleanType);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>xor</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IOldTypeReference XorOperator(IOldTypeReference value1, IOldTypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return ToBoolean(BooleanValueBase.Xor(boolean1, boolean2), KnownTypeIds.BooleanType);
            else
                return Invalid;

        }

        /// <summary>
        ///     get a byte bool value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public IBooleanValue ToByteBool(byte value, int typeId)
            => new ByteBooleanValue(value, typeId);

        /// <summary>
        ///     get a word bool value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public IBooleanValue ToWordBool(ushort value, int typeId)
            => new WordBooleanValue(value, typeId);

        /// <summary>
        ///     get a word bool value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public IBooleanValue ToLongBool(uint value, int typeId)
            => new LongBooleanValue(value, typeId);


        /// <summary>
        ///     get a new boolean value depending on the bit size
        /// </summary>
        /// <param name="bitSize"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOldTypeReference ToBoolean(uint bitSize, uint value) {
            switch (bitSize) {
                case 1:
                    return value == 0 ? FalseValue : TrueValue;

                case 8:
                    return ToByteBool((byte)value, KnownTypeIds.BooleanType);

                case 16:
                    return ToWordBool((ushort)value, KnownTypeIds.WordBoolType);

                case 32:
                    return ToLongBool(value, KnownTypeIds.LongBoolType);

                default:
                    return Invalid;
            }
        }
    }
}
