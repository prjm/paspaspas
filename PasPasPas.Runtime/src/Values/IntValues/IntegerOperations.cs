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
        public IOldTypeReference Invalid { get; }
            = new SpecialValue(SpecialConstantKind.InvalidInteger, KnownTypeIds.ErrorType);

        /// <summary>
        ///     integer overflow
        /// </summary>
        public IOldTypeReference Overflow { get; }
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
        public IOldTypeReference Zero
            => new ShortIntValue(0);

        /// <summary>
        ///     one / integral value
        /// </summary>
        public IOldTypeReference One
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
        public IOldTypeReference Add(IOldTypeReference augend, IOldTypeReference addend) {
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
        public IOldTypeReference AndOperator(IOldTypeReference firstOperand, IOldTypeReference secondOperand) {
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
        public IOldTypeReference Divide(IOldTypeReference dividend, IOldTypeReference divisor) {
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
        public IOldTypeReference Equal(IOldTypeReference left, IOldTypeReference right) {
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.Equal(firstInt, secondInt), KnownTypeIds.BooleanType);
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
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.GreaterThen(firstInt, secondInt), KnownTypeIds.BooleanType);
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
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.GreaterThenEqual(firstInt, secondInt), KnownTypeIds.BooleanType);
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
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.LessThen(firstInt, secondInt), KnownTypeIds.BooleanType);
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
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.LessThenEqual(firstInt, secondInt), KnownTypeIds.BooleanType);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference NotEquals(IOldTypeReference left, IOldTypeReference right) {
            if (left is IntegerValueBase firstInt && right is IntegerValueBase secondInt)
                return Booleans.ToBoolean(IntegerValueBase.NotEqual(firstInt, secondInt), KnownTypeIds.BooleanType);
            else
                return Invalid;

        }

        /// <summary>
        ///     calculate an integer remainder
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public IOldTypeReference Modulo(IOldTypeReference dividend, IOldTypeReference divisor) {
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
        public IOldTypeReference Multiply(IOldTypeReference multiplicand, IOldTypeReference intMultiplier) {
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
        public IOldTypeReference Negate(IOldTypeReference number) {
            if (number is IntegerValueBase intNumber)
                return IntegerValueBase.Negate(Overflow, intNumber);
            else
                return Invalid;
        }

        /// <summary>
        ///     invert a number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOldTypeReference NotOperator(IOldTypeReference value) {
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
        public IOldTypeReference Identity(IOldTypeReference number)
            => number;

        /// <summary>
        ///     compute a bitwise or
        /// </summary>
        /// <param name="value1">first operand</param>
        /// <param name="value2">second operand</param>
        /// <returns></returns>
        public IOldTypeReference OrOperator(IOldTypeReference value1, IOldTypeReference value2) {
            if (value1 is IntegerValueBase firstInt && value2 is IntegerValueBase secondInt)
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
        public IOldTypeReference Shl(IOldTypeReference firstOperand, IOldTypeReference secondOperand) {
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
        public IOldTypeReference Shr(IOldTypeReference firstOperand, IOldTypeReference secondOperand) {
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
        public IOldTypeReference Subtract(IOldTypeReference minuend, IOldTypeReference subtrahend) {
            if (minuend is IntegerValueBase intMinuend && subtrahend is IntegerValueBase intSubtrahend)
                return IntegerValueBase.SubtractAndScale(Overflow, intMinuend, intSubtrahend);
            else
                return Invalid;
        }

        /// <summary>
        ///     xor two integers
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IOldTypeReference XorOperator(IOldTypeReference value1, IOldTypeReference value2) {
            if (value1 is IntegerValueBase firstInt && value2 is IntegerValueBase secondInt)
                return IntegerValueBase.XorAndScale(Overflow, firstInt, secondInt);
            else
                return Invalid;
        }

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToScaledIntegerValue(sbyte number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToScaledIntegerValue(byte number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToScaledIntegerValue(short number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToScaledIntegerValue(ushort number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToScaledIntegerValue(int number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToScaledIntegerValue(uint number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToScaledIntegerValue(long number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert an integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToScaledIntegerValue(ulong number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     increment an integer value
        /// </summary>
        /// <param name="value">value to increment</param>
        /// <returns></returns>
        public IOldTypeReference Increment(IOldTypeReference value) {
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
        public IOldTypeReference ToIntegerValue(sbyte number)
            => new ShortIntValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToIntegerValue(byte number)
            => new ByteValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToIntegerValue(short number)
            => new SmallIntValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToIntegerValue(ushort number)
            => new WordValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToIntegerValue(int number)
            => new IntegerValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToIntegerValue(uint number)
            => new CardinalValue(number);

        /// <summary>
        ///     get a fixed value for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToIntegerValue(long number)
            => new Int64Value(number);

        /// <summary>
        ///     get a
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public IOldTypeReference ToIntegerValue(ulong number)
            => new UInt64Value(number);

        /// <summary>
        ///     absolute value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOldTypeReference Abs(IOldTypeReference value) {
            if (value is IntegerValueBase integerValue)
                return IntegerValueBase.AbsoluteValue(Overflow, integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>chr</c> function
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public IOldTypeReference Chr(IOldTypeReference typeReference) {
            if (typeReference is IntegerValueBase integerValue)
                return IntegerValueBase.ChrValue(integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>hi</c> function
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public IOldTypeReference Hi(IOldTypeReference typeReference) {
            if (typeReference is IntegerValueBase integerValue)
                return IntegerValueBase.HiValue(integerValue);
            else
                return Invalid;
        }


        /// <summary>
        ///     <c>lo</c> function
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public IOldTypeReference Lo(IOldTypeReference typeReference) {
            if (typeReference is IntegerValueBase integerValue)
                return IntegerValueBase.LoValue(integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     swap high and low byte
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public IOldTypeReference Swap(IOldTypeReference parameter, ITypeRegistry types) {
            if (parameter is IntegerValueBase integerValue)
                return IntegerValueBase.Swap(Overflow, Invalid, types, integerValue);
            else
                return Invalid;
        }

        /// <summary>
        ///     convert a number to a native int value
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeRegistry"></param>
        /// <returns></returns>
        public IOldTypeReference ToNativeInt(IOldTypeReference number, ITypeRegistry typeRegistry) {
            var nativeType = typeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.NativeInt) as IAliasedType;

            if (nativeType == default)
                return Invalid;

            var intType = typeRegistry.GetTypeByIdOrUndefinedType(nativeType.BaseTypeId) as IOrdinalType;

            if (intType == default)
                return Invalid;

            if (!number.IsIntegralValue(out var integerValue))
                return Invalid;

            if (intType.BitSize == 32)
                return ToIntegerValue((int)integerValue.SignedValue);

            if (intType.BitSize == 64)
                return ToIntegerValue(integerValue.SignedValue);

            return Invalid;
        }


    }
}
