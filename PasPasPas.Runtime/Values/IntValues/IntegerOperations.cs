using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;

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
        public ITypeReference And(ITypeReference firstOperand, ITypeReference secondOperand) {
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
                return Booleans.AsBoolean(IntegerValueBase.Equal(firstInt, secondInt));
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
                return Booleans.AsBoolean(IntegerValueBase.GreaterThen(firstInt, secondInt));
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
                return Booleans.AsBoolean(IntegerValueBase.GreaterThenEqual(firstInt, secondInt));
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
                return Booleans.AsBoolean(IntegerValueBase.LessThen(firstInt, secondInt));
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
                return Booleans.AsBoolean(IntegerValueBase.LessThenEqual(firstInt, secondInt));
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
                return Booleans.AsBoolean(IntegerValueBase.NotEqual(firstInt, secondInt));
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
        public ITypeReference Not(ITypeReference number) {
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
        public ITypeReference Or(ITypeReference firstOperand, ITypeReference secondOperand) {
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
        public ITypeReference Xor(ITypeReference firstOperand, ITypeReference secondOperand) {
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
        ///     cast a value to another type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeReference Cast(ITypeReference value, int typeId) {

            if (!(value is IIntegerValue integer))
                return Types.MakeReference(KnownTypeIds.ErrorType);

            switch (typeId) {
                case KnownTypeIds.ShortInt:
                    return new ShortIntValue((sbyte)integer.SignedValue);
                case KnownTypeIds.ByteType:
                    return new ByteValue((byte)integer.UnsignedValue);
                case KnownTypeIds.SmallInt:
                    return new SmallIntValue((short)integer.SignedValue);
                case KnownTypeIds.WordType:
                    return new WordValue((ushort)integer.UnsignedValue);
                case KnownTypeIds.IntegerType:
                    return new IntegerValue((int)integer.SignedValue);
                case KnownTypeIds.CardinalType:
                    return new CardinalValue((uint)integer.UnsignedValue);
                case KnownTypeIds.Int64Type:
                    return new Int64Value(integer.SignedValue);
                case KnownTypeIds.Uint64Type:
                    return new UInt64Value(integer.UnsignedValue);
            }

            return Types.MakeReference(KnownTypeIds.ErrorType);
        }
    }
}
