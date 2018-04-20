using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Int {

    /// <summary>
    ///     integer value factory and operations
    /// </summary>
    public class IntegerOperations : IIntegerOperations {

        /// <summary>
        ///     invalid integer
        /// </summary>
        public IValue Invalid { get; }
            = new SpecialValue(SpecialConstantKind.InvalidInteger);

        /// <summary>
        ///     integer overflow
        /// </summary>
        public IValue Overflow { get; }
            = new SpecialValue(SpecialConstantKind.IntegerOverflow);

        /// <summary>
        ///     calculate the sum of two integers
        /// </summary>
        /// <param name="augend">first operand</param>
        /// <param name="addend">second operand</param>
        /// <returns>sum</returns>
        public IValue Add(IValue augend, IValue addend) {
            if (augend is IntegerValueBase intAugend && addend is IntegerValueBase intAddend)
                return IntegerValueBase.AddAndScale(intAugend, intAddend);
            else
                return Invalid;
        }

        /// <summary>
        ///     calculate the bitwise and of two integers
        /// </summary>
        /// <param name="firstOperand">first operand</param>
        /// <param name="secondOperand">second operand</param>
        /// <returns>bitwise and</returns>
        public IValue And(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.AndAndScale(firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     calculate an integer division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public IValue Divide(IValue dividend, IValue divisor) {
            if (dividend is IntegerValueBase intDividend && divisor is IntegerValueBase intDivisor)
                if (intDivisor.SignedValue == 0)
                    return new SpecialValue(SpecialConstantKind.DivisionByZero);
                else
                    return IntegerValueBase.DivideAndScale(intDividend, intDivisor);
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
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return IntegerValueBase.Equal(firstInt, secondInt);
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
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return IntegerValueBase.GreaterThen(firstInt, secondInt);
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
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return IntegerValueBase.GreaterThenEqual(firstInt, secondInt);
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
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return IntegerValueBase.LessThen(firstInt, secondInt);
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
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return IntegerValueBase.LessThenEqual(firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue NotEquals(IValue left, IValue right) {
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return IntegerValueBase.NotEqual(firstInt, secondInt);
            else
                return Invalid;

        }

        /// <summary>
        ///     calculate an integer remainder
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public IValue Modulo(IValue dividend, IValue divisor) {
            if (dividend is IntegerValueBase intDividend && divisor is IntegerValueBase intDivisor)
                if (intDivisor.SignedValue == 0)
                    return new SpecialValue(SpecialConstantKind.DivisionByZero);
                else
                    return IntegerValueBase.ModuloAndScale(intDividend, intDivisor);
            else
                return Invalid;
        }

        /// <summary>
        ///     multiply two integer values
        /// </summary>
        /// <param name="multiplicand">multiplicand</param>
        /// <param name="intMultiplier">multiplier</param>
        /// <returns></returns>
        public IValue Multiply(IValue multiplicand, IValue intMultiplier) {
            if (multiplicand is IntegerValueBase intMultiplicand && intMultiplier is IntegerValueBase secondInt)
                return IntegerValueBase.MultiplyAndScale(intMultiplicand, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     negate a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue Negate(IValue number) {
            if (number is IntegerValueBase intNumber)
                return IntegerValueBase.Negate(intNumber);
            else
                return Invalid;
        }

        /// <summary>
        ///     invert a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue Not(IValue number) {
            if (number is IntegerValueBase intNumber)
                return IntegerValueBase.Not(intNumber);
            else
                return Invalid;
        }



        /// <summary>
        ///     compute a bitwise or
        /// </summary>
        /// <param name="firstOperand">first operand</param>
        /// <param name="secondOperand">second operand</param>
        /// <returns></returns>
        public IValue Or(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.OrAndScale(firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     compute a left shift
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public IValue Shl(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.ShlAndScale(firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     compute a right shift
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public IValue Shr(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.ShrAndScale(firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     subtract two numbers
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns></returns>
        public IValue Subtract(IValue minuend, IValue subtrahend) {
            if (minuend is IntegerValueBase intMinuend && subtrahend is IntegerValueBase intSubtrahend)
                return IntegerValueBase.SubtractAndScale(intMinuend, intSubtrahend);
            else
                return Invalid;
        }

        /// <summary>
        ///     xor two integers
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public IValue Xor(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.XorAndScale(firstInt, secondInt);
            else
                return Invalid;
        }

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



    }
}
