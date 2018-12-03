using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     integer value factory and operations
    /// </summary>
    public class IntegerOperations : IIntegerOperations {

        /// <summary>
        ///     invalid integer
        /// </summary>
        public ITypeReference Invalid { get; }
            = new SpecialValue(SpecialConstantKind.InvalidInteger, KnownTypeIds.ErrorType);

        /// <summary>
        ///     integer overflow
        /// </summary>
        public ITypeReference Overflow { get; }
            = new SpecialValue(SpecialConstantKind.IntegerOverflow, KnownTypeIds.ErrorType);

        /// <summary>
        ///     boolean operations
        /// </summary>
        public IBooleanOperations Booleans { get; }

        /// <summary>
        ///     registered types
        /// </summary>
        public ITypeOperations Types { get; }

        /// <summary>
        ///     zero / default value
        /// </summary>
        public ITypeReference Zero
            => new ShortIntValue(0);

        /// <summary>
        ///     one / integral value
        /// </summary>
        public ITypeReference One
            => new ShortIntValue(1);

        /// <summary>
        ///     create a new integer operations helper
        /// </summary>
        /// <param name="booleans">boolean operations</param>
        /// <param name="types">runtime types</param>
        public IntegerOperations(IBooleanOperations booleans, ITypeOperations types) {
            Booleans = booleans;
            Types = types;
        }

        /// <summary>1
        ///     calculate the sum of two integers
        /// </summary>
        /// <param name="augend">first operand</param>
        /// <param name="addend">second operand</param>
        /// <returns>sum</returns>
        public ITypeReference Add(ITypeReference augend, ITypeReference addend) {
            if (augend is IntegerValueBase intAugend && addend is IntegerValueBase intAddend)
                return IntegerValueBase.AddAndScale(Overflow, intAugend, intAddend);
            else
                return Invalid;
        }

        /// <summary>
        ///     calculate the bitwise and of two integers
        /// </summary>
        /// <param name="firstOperand">first operand</param>
        /// <param name="secondOperand">second operand</param>
        /// <returns>bitwise and</returns>
        public ITypeReference AndOperator(ITypeReference firstOperand, ITypeReference secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.AndAndScale(Overflow, firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     calculate an integer division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public ITypeReference Divide(ITypeReference dividend, ITypeReference divisor) {
            if (dividend is IntegerValueBase intDividend && divisor is IntegerValueBase intDivisor)
                if (intDivisor.SignedValue == 0)
                    return new SpecialValue(SpecialConstantKind.DivisionByZero);
                else
                    return IntegerValueBase.DivideAndScale(Overflow, intDividend, intDivisor);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>==</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference Equal(ITypeReference left, ITypeReference right) {
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.Equal(firstInt, secondInt));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference GreaterThen(ITypeReference left, ITypeReference right) {
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.GreaterThen(firstInt, secondInt));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference GreaterThenEqual(ITypeReference left, ITypeReference right) {
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.GreaterThenEqual(firstInt, secondInt));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference LessThen(ITypeReference left, ITypeReference right) {
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.LessThen(firstInt, secondInt));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference LessThenOrEqual(ITypeReference left, ITypeReference right) {
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.LessThenEqual(firstInt, secondInt));
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference NotEquals(ITypeReference left, ITypeReference right) {
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.NotEqual(firstInt, secondInt));
            else
                return Invalid;

        }

        /// <summary>
        ///     calculate an integer remainder
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public ITypeReference Modulo(ITypeReference dividend, ITypeReference divisor) {
            if (dividend is IntegerValueBase intDividend && divisor is IntegerValueBase intDivisor)
                if (intDivisor.SignedValue == 0)
                    return new SpecialValue(SpecialConstantKind.DivisionByZero);
                else
                    return IntegerValueBase.ModuloAndScale(Overflow, intDividend, intDivisor);
            else
                return Invalid;
        }

        /// <summary>
        ///     multiply two integer values
        /// </summary>
        /// <param name="multiplicand">multiplicand</param>
        /// <param name="intMultiplier">multiplier</param>
        /// <returns></returns>
        public ITypeReference Multiply(ITypeReference multiplicand, ITypeReference intMultiplier) {
            if (multiplicand is IntegerValueBase intMultiplicand && intMultiplier is IntegerValueBase secondInt)
                return IntegerValueBase.MultiplyAndScale(Overflow, intMultiplicand, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     negate a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference Negate(ITypeReference number) {
            if (number is IntegerValueBase intNumber)
                return IntegerValueBase.Negate(Overflow, intNumber);
            else
                return Invalid;
        }

        /// <summary>
        ///     invert a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference NotOperator(ITypeReference number) {
            if (number is IntegerValueBase intNumber)
                return IntegerValueBase.Not(intNumber);
            else
                return Invalid;
        }

        /// <summary>
        ///     identity function
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference Identity(ITypeReference number)
            => number;

        /// <summary>
        ///     compute a bitwise or
        /// </summary>
        /// <param name="firstOperand">first operand</param>
        /// <param name="secondOperand">second operand</param>
        /// <returns></returns>
        public ITypeReference OrOperator(ITypeReference firstOperand, ITypeReference secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.OrAndScale(Overflow, firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     compute a left shift
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public ITypeReference Shl(ITypeReference firstOperand, ITypeReference secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.ShlAndScale(Overflow, firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     compute a right shift
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public ITypeReference Shr(ITypeReference firstOperand, ITypeReference secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.ShrAndScale(Overflow, firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     subtract two numbers
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns></returns>
        public ITypeReference Subtract(ITypeReference minuend, ITypeReference subtrahend) {
            if (minuend is IntegerValueBase intMinuend && subtrahend is IntegerValueBase intSubtrahend)
                return IntegerValueBase.SubtractAndScale(Overflow, intMinuend, intSubtrahend);
            else
                return Invalid;
        }

        /// <summary>
        ///     xor two integers
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        public ITypeReference XorOperator(ITypeReference firstOperand, ITypeReference secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return IntegerValueBase.XorAndScale(Overflow, firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToScaledIntegerValue(sbyte number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToScaledIntegerValue(byte number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToScaledIntegerValue(short number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToScaledIntegerValue(ushort number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToScaledIntegerValue(int number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToScaledIntegerValue(uint number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToScaledIntegerValue(long number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToScaledIntegerValue(ulong number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     increment an integer value
        /// </summary>
        /// <param name="value">value to increment</param>
        /// <returns></returns>
        public ITypeReference Increment(ITypeReference value) {
            if (value is IntegerValueBase integerValue)
                return IntegerValueBase.Increment(Overflow, integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToIntegerValue(sbyte number)
            => new ShortIntValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToIntegerValue(byte number)
            => new ByteValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToIntegerValue(short number)
            => new SmallIntValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToIntegerValue(ushort number)
            => new WordValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToIntegerValue(int number)
            => new IntegerValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToIntegerValue(uint number)
            => new CardinalValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToIntegerValue(long number)
            => new Int64Value(number);

        /// <summary>
        ///     get a
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ITypeReference ToIntegerValue(ulong number)
            => new UInt64Value(number);

        /// <summary>
        ///     absolute value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference Abs(ITypeReference value) {
            if (value is IntegerValueBase integerValue)
                return IntegerValueBase.AbsoluteValue(Overflow, integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>chr</c> function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference Chr(ITypeReference value) {
            if (value is IntegerValueBase integerValue)
                return IntegerValueBase.ChrValue(integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>hi</c> function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference Hi(ITypeReference value) {
            if (value is IntegerValueBase integerValue)
                return IntegerValueBase.HiValue(integerValue);
            else
                return Invalid;
        }
    }
}
