using System;
using PasPasPas.Global.Runtime;
using PasPasPas.Runtime.Values.Boolean;
using PasPasPas.Runtime.Values.Char;
using PasPasPas.Runtime.Values.Float;
using PasPasPas.Runtime.Values.Int;
using PasPasPas.Runtime.Values.String;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     constant operations helper
    /// </summary>
    public class RuntimeValues : IRuntimeValues {

        private readonly IValue falseValue
            = new BooleanValue(false);

        private readonly IValue trueValue
            = new BooleanValue(true);

        private readonly IntegerCalculator integerCalculator
            = new IntegerCalculator();

        private readonly FloatCalculator extendedCalculator
            = new FloatCalculator();

        private readonly BooleanCalculator booleanCalculator
            = new BooleanCalculator();

        private readonly StringCalculator stringCalculator
            = new StringCalculator();

        /// <summary>
        ///     integer calculator
        /// </summary>
        public IIntegerCalculator ScaledIntegerCalculator
            => integerCalculator;

        /// <summary>
        ///     calculator for extended values
        /// </summary>
        public IFloatCalculator FloatCalculator
            => extendedCalculator;

        /// <summary>
        ///     get the boolean calculator
        /// </summary>
        public IBooleanCalculator BooleanCalculator
            => booleanCalculator;

        /// <summary>
        ///     get the string calculator
        /// </summary>
        public IStringCalculator StringCalculator
            => stringCalculator;

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(sbyte number)
            => new ShortIntValue(number);

        /// <summary>
        ///     convert a byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(byte number) {
            if (number < 128)
                return new ShortIntValue((sbyte)number);

            return new ByteValue(number);
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(int number) {
            if (number < -32768)
                return new IntegerValue(number);
            else if (number < -128)
                return new SmallIntValue((short)number);
            else if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);
            else if (number < 65536)
                return new WordValue((ushort)number);

            return new IntegerValue(number);
        }

        /// <summary>
        ///     convert a unsigned int a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(uint number) {
            if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);
            else if (number < 65536)
                return new WordValue((ushort)number);
            else if (number < 2147483648)
                return new IntegerValue((int)number);

            return new CardinalValue(number);
        }

        /// <summary>
        ///     convert a long to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(long number) {
            if (number < -2147483648)
                return new Int64Value(number);
            else if (number < -32768)
                return new IntegerValue((int)number);
            else if (number < -128)
                return new SmallIntValue((short)number);
            else if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);
            else if (number < 65536)
                return new WordValue((ushort)number);
            else if (number < 2147483648)
                return new IntegerValue((int)number);
            else if (number < 4294967296)
                return new CardinalValue((uint)number);

            return new Int64Value(number);
        }

        /// <summary>
        ///     convert a unsigned long to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(ulong number) {
            if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);
            else if (number < 65536)
                return new WordValue((ushort)number);
            else if (number < 2147483648)
                return new IntegerValue((int)number);
            else if (number < 4294967296)
                return new CardinalValue((uint)number);
            else if (number < 9223372036854775808)
                return new Int64Value((long)number);

            return new UInt64Value(number);
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(short number) {
            if (number < -128)
                return new SmallIntValue(number);
            else if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);

            return new SmallIntValue(number);
        }

        /// <summary>
        ///     convert a byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(ushort number) {
            if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);

            return new WordValue(number);
        }

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
        public IValue ToExtendedValue(in ExtF80 value)
            => new ExtendedValue(value);

        /// <summary>
        ///     get a constant wide char value
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public IValue ToWideCharValue(char character)
            => new WideCharValue(character);

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
                    case SpecialConstantKind.InvalidBool:
                        return new SpecialValue(SpecialConstantKind.InvalidInteger);
                    default:
                        throw new IndexOutOfRangeException();
                };
            }
        }
    }
}