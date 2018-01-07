using System;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     constant operations helper
    /// </summary>
    public class RuntimeValues : IRuntimeValues {

        private readonly IValue falseValue
            = new BooleanValue(false);

        private readonly IValue trueValue
            = new BooleanValue(true);

        /// <summary>
        ///     negate a value
        /// </summary>
        /// <param name="value">value to negate</param>
        /// <returns>negated value</returns>
        public object Negate(object value) {
            if (value is sbyte i8)
                return ToConstantInt(-i8);
            else if (value is byte ui8)
                return ToConstantInt(-ui8);
            else if (value is short i16)
                return ToConstantInt(-i16);
            else if (value is ushort ui16)
                return ToConstantInt(-ui16);
            else if (value is int i32)
                return ToConstantInt(-i32);
            else if (value is uint ui32)
                return ToConstantInt(-ui32);
            if (value is long i64)
                return ToConstantInt(-i64);
            if (value is ulong ui64 && ui64 <= 9223372036854775807)
                return ToConstantInt(-((long)ui64));
            if (value is double d)
                return -d;

            throw new InvalidArithmeticOperationException("Negate", value);
        }

        /// <summary>
        ///     convert an integral number to an internal literal
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public object ToConstantInt(long result) {
            if (result < -2147483648)
                return result;
            else if (result < -32768)
                return (int)result;
            else if (result < -128)
                return (short)result;
            else if (result < 128)
                return (sbyte)result;
            else if (result < 256)
                return (byte)result;
            else if (result < 32768)
                return (short)result;
            else if (result < 65536)
                return (ushort)result;
            else if (result < 2147483648)
                return (int)result;
            else if (result < 4294967296)
                return (uint)result;

            return result;
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(sbyte number)
            => new ScaledIntegerValue(number);

        /// <summary>
        ///     convert a byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(byte number)
            => new ScaledIntegerValue(number);

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(int number)
            => new ScaledIntegerValue(number);

        /// <summary>
        ///     convert a unsigned int a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(uint number)
            => new ScaledIntegerValue(number);

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(long number)
            => new ScaledIntegerValue(number);

        /// <summary>
        ///     convert a byteto a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(ulong number)
            => new ScaledIntegerValue(number);

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(short number)
            => new ScaledIntegerValue(number);

        /// <summary>
        ///     convert a byteto a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(ushort number)
            => new ScaledIntegerValue(number);

        /// <summary>
        ///     get a constant char value
        /// </summary>
        /// <param name="singleChar"></param>
        /// <returns></returns>
        public IValue ToWideCharValue(char singleChar)
            => new WideCharValue(singleChar);

        /// <summary>
        ///     get a constant text value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IValue ToUnicodeString(string text)
            => new UnicodeStringValue(text);

        /// <summary>
        ///     get a constant real value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValue ToExtendedValue(double value)
            => new FloatingPointValue(FloatingPointValueKind.Extended, value);

        /// <summary>
        ///     get a special constant value
        /// </summary>
        /// <param name="index">kind of constant value</param>
        /// <returns></returns>
        public IValue this[SpecialConstantKind index] {
            get {
                switch (index) {
                    case SpecialConstantKind.FalseValue:
                        return falseValue;
                    case SpecialConstantKind.TrueValue:
                        return trueValue;
                    case SpecialConstantKind.IntegerOverflow:
                        return new SpecialValue(SpecialConstantKind.IntegerOverflow);
                    case SpecialConstantKind.InvalidReal:
                        return new SpecialValue(SpecialConstantKind.InvalidReal);
                    case SpecialConstantKind.InvalidInteger:
                        return new SpecialValue(SpecialConstantKind.InvalidInteger);
                    default:
                        throw new IndexOutOfRangeException();
                };
            }
        }
    }
}