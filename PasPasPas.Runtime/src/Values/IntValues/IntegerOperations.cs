using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     integer value factory and operations
    /// </summary>
    public class IntegerOperations : IIntegerOperations {

        private readonly ITypeRegistryProvider provider;

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
        ///     boolean operations
        /// </summary>
        public IBooleanOperations Booleans { get; }

        /// <summary>
        ///     registered types
        /// </summary>
        public ITypeOperations Types { get; }

        private readonly Lazy<ShortIntValue> zero;

        /// <summary>
        ///     zero / default value
        /// </summary>
        public IValue Zero
            => zero.Value;

        private readonly Lazy<ShortIntValue> one;

        /// <summary>
        ///     one / integral value
        /// </summary>
        public IValue One
            => one.Value;

        /// <summary>
        ///     create a new integer operations helper
        /// </summary>
        /// <param name="booleans">boolean operations</param>
        /// <param name="types">runtime types</param>
        /// <param name="typeProvider"></param>
        public IntegerOperations(ITypeRegistryProvider typeProvider, IBooleanOperations booleans, ITypeOperations types) {
            Booleans = booleans;
            Types = types;
            provider = typeProvider;
            zero = new Lazy<ShortIntValue>(() => new ShortIntValue(provider.GetShortIntType(), 0));
            one = new Lazy<ShortIntValue>(() => new ShortIntValue(provider.GetShortIntType(), 1));
        }

        /// <summary>1
        ///     calculate the sum of two integers
        /// </summary>
        /// <param name="augend">first operand</param>
        /// <param name="addend">second operand</param>
        /// <returns>sum</returns>
        public IValue Add(IValue augend, IValue addend) {
            if (augend is IntegerValueBase intAugend && addend is IntegerValueBase intAddend)
                return intAugend.AddAndScale(Overflow, intAugend, intAddend);
            else
                return Invalid;
        }

        /// <summary>
        ///     calculate the bitwise and of two integers
        /// </summary>
        /// <param name="firstOperand">first operand</param>
        /// <param name="secondOperand">second operand</param>
        /// <returns>bitwise and</returns>
        public IValue AndOperator(IValue firstOperand, IValue secondOperand) {
            if (firstOperand is IntegerValueBase firstInt && secondOperand is IntegerValueBase secondInt)
                return firstInt.AndAndScale(Overflow, firstInt, secondInt);
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
                    return intDividend.DivideAndScale(Overflow, intDividend, intDivisor);
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
        public IValue GreaterThen(IValue left, IValue right) {
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
        public IValue GreaterThenEqual(IValue left, IValue right) {
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
        public IValue LessThen(IValue left, IValue right) {
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
        public IValue LessThenOrEqual(IValue left, IValue right) {
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
        public IValue NotEquals(IValue left, IValue right) {
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
        public IValue Modulo(IValue dividend, IValue divisor) {
            if (dividend is IntegerValueBase intDividend && divisor is IntegerValueBase intDivisor)
                if (intDivisor.SignedValue == 0)
                    return new SpecialValue(SpecialConstantKind.DivisionByZero);
                else
                    return intDivisor.ModuloAndScale(Overflow, intDividend, intDivisor);
            else
                return Invalid;
        }

        /// <summary>
        ///     multiply two integer values
        /// </summary>
        /// <param name="multiplicand">multiplicand</param>
        /// <param name="multiplier">multiplier</param>
        /// <returns></returns>
        public IValue Multiply(IValue multiplicand, IValue multiplier) {
            if (multiplicand is IntegerValueBase intMultiplicand && multiplier is IntegerValueBase intMultiplier)
                return intMultiplicand.MultiplyAndScale(Overflow, intMultiplicand, intMultiplier);
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
                return intNumber.Negate(Overflow, intNumber);
            else
                return Invalid;
        }

        /// <summary>
        ///     invert a number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValue NotOperator(IValue value) {
            if (value is IntegerValueBase intNumber)
                return IntegerValueBase.Not(intNumber);
            else
                return Invalid;
        }

        /// <summary>
        ///     identity function
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue Identity(IValue number)
            => number;

        /// <summary>
        ///     compute a bitwise or
        /// </summary>
        /// <param name="value1">first operand</param>
        /// <param name="value2">second operand</param>
        /// <returns></returns>
        public IValue OrOperator(IValue value1, IValue value2) {
            if (value1 is IntegerValueBase firstInt && value2 is IntegerValueBase secondInt)
                return firstInt.OrAndScale(Overflow, firstInt, secondInt);
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
                return firstInt.ShlAndScale(Overflow, firstInt, secondInt);
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
                return firstInt.ShrAndScale(Overflow, firstInt, secondInt);
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
                return intMinuend.SubtractAndScale(Overflow, intMinuend, intSubtrahend);
            else
                return Invalid;
        }

        /// <summary>
        ///     xor two integers
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue XorOperator(IValue value1, IValue value2) {
            if (value1 is IntegerValueBase firstInt && value2 is IntegerValueBase secondInt)
                return firstInt.XorAndScale(Overflow, firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(sbyte number)
            => zero.Value.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(byte number)
            => zero.Value.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(short number)
            => zero.Value.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(ushort number)
            => zero.Value.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(int number)
            => zero.Value.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(uint number)
            => zero.Value.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(long number)
            => zero.Value.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(ulong number)
            => zero.Value.ToScaledIntegerValue(number);

        /// <summary>
        ///     increment an integer value
        /// </summary>
        /// <param name="value">value to increment</param>
        /// <returns></returns>
        public IValue Increment(IValue value) {
            if (value is IntegerValueBase integerValue)
                return integerValue.Increment(Overflow, integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToIntegerValue(sbyte number)
            => new ShortIntValue(provider.GetShortIntType(), number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToIntegerValue(byte number)
            => new ByteValue(provider.GetByteType(), number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToIntegerValue(short number)
            => new SmallIntValue(provider.GetSmallIntType(), number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToIntegerValue(ushort number)
            => new WordValue(provider.GetWordType(), number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToIntegerValue(int number)
            => new IntegerValue(provider.GetIntegerType(), number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToIntegerValue(uint number)
            => new CardinalValue(provider.GetCardinalType(), number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToIntegerValue(long number)
            => new Int64Value(provider.GetInt64Type(), number);

        /// <summary>
        ///     get a
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IValue ToIntegerValue(ulong number)
            => new UInt64Value(provider.GetUInt64Type(), number);

        /// <summary>
        ///     absolute value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValue Abs(IValue value) {
            if (value is IntegerValueBase integerValue)
                return integerValue.AbsoluteValue(Overflow, integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>chr</c> function
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public IValue Chr(IValue typeReference) {
            if (typeReference is IntegerValueBase integerValue)
                return integerValue.ChrValue(integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>hi</c> function
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public IValue Hi(IValue typeReference) {
            if (typeReference is IntegerValueBase integerValue)
                return integerValue.HiValue(integerValue);
            else
                return Invalid;
        }


        /// <summary>
        ///     <c>lo</c> function
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public IValue Lo(IValue typeReference) {
            if (typeReference is IntegerValueBase integerValue)
                return integerValue.LoValue(integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     swap high and low byte
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public IValue Swap(IValue parameter, ITypeRegistry types) {
            if (parameter is IntegerValueBase integerValue)
                return integerValue.Swap(Overflow, Invalid, integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     convert a number to a native int value
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeRegistry"></param>
        /// <returns></returns>
        public IValue ToNativeInt(IValue number, ITypeRegistry typeRegistry) {
            var nativeType = typeRegistry.SystemUnit.NativeIntType as IAliasedType;

            if (nativeType == default)
                return Invalid;

            var intType = nativeType.BaseTypeDefinition as IOrdinalType;

            if (intType == default)
                return Invalid;

            if (!(number is IIntegerValue integerValue))
                return Invalid;

            if (intType.TypeSizeInBytes == 4)
                return ToIntegerValue((int)integerValue.SignedValue);

            if (intType.TypeSizeInBytes == 8)
                return ToIntegerValue(integerValue.SignedValue);

            return Invalid;
        }


    }
}
