using System;
using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.CharValues;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     base class for integer values
    /// </summary>
    internal abstract class IntegerValueBase : RuntimeValueBase, IIntegerValue {

        /// <summary>
        ///     create a new integer value
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="kind"></param>
        protected IntegerValueBase(ITypeDefinition typeDef, IntegralTypeKind kind) : base(typeDef) {
            if (typeDef.BaseType != BaseType.Integer && typeDef.BaseType != BaseType.Enumeration)
                throw new ArgumentException(string.Empty, nameof(typeDef));

            var intType = typeDef as IIntegralType;
            var enumType = typeDef as IEnumeratedType;

            if (intType == default && enumType == default)
                throw new ArgumentException(string.Empty, nameof(typeDef));

            if (intType != default && intType.Kind != kind)
                throw new ArgumentException(string.Empty, nameof(typeDef));

        }

        /// <summary>
        ///     check if the value is negative
        /// </summary>
        public abstract bool IsNegative { get; }

        /// <summary>
        ///     numerical value
        /// </summary>
        public abstract long SignedValue { get; }

        /// <summary>
        ///     numerical unsigned value
        /// </summary>
        public abstract ulong UnsignedValue { get; }

        /// <summary>
        ///     get the numerical value
        /// </summary>
        public abstract BigInteger AsBigInteger { get; }

        /// <summary>
        ///     get the value as extended value
        /// </summary>
        public ExtF80 AsExtended {
            get {
                if (((IIntegralType)TypeDefinition).Kind == IntegralTypeKind.UInt64)
                    return UnsignedValue;

                return SignedValue;
            }
        }

        /// <summary>
        ///     get the integral type definition
        /// </summary>
        public IIntegralType IntegralType
            => TypeDefinition as IIntegralType ?? throw new InvalidOperationException();

        /// <summary>
        ///     convert a big integer value to an integer value
        /// </summary>
        /// <param name="overflow">value used for overflow</param>
        /// <param name="value">value to convert</param>
        /// <returns>converted value</returns>
        public IValue ToIntValue(IValue overflow, BigInteger value) {
            if (value < -9223372036854775808)
                return overflow;
            else if (value < -2147483648)
                return new Int64Value(SystemUnit.Int64Type, (long)value);
            else if (value < -32768)
                return new IntegerValue(SystemUnit.IntegerType, (int)value);
            else if (value < -128)
                return new SmallIntValue(SystemUnit.SmallIntType, (short)value);
            else if (value < 128)
                return new ShortIntValue(SystemUnit.ShortIntType, (sbyte)value);
            else if (value < 256)
                return new ByteValue(SystemUnit.ByteType, (byte)value);
            else if (value < 32768)
                return new SmallIntValue(SystemUnit.SmallIntType, (short)value);
            else if (value < 65536)
                return new WordValue(SystemUnit.WordType, (ushort)value);
            else if (value < 2147483648)
                return new IntegerValue(SystemUnit.IntegerType, (int)value);
            else if (value < 4294967296)
                return new CardinalValue(SystemUnit.CardinalType, (uint)value);
            else if (value < 9223372036854775808)
                return new Int64Value(SystemUnit.Int64Type, (long)value);
            else if (value <= 18446744073709551615)
                return new UInt64Value(SystemUnit.UInt64Type, (ulong)value);

            return overflow;
        }

        internal IValue AddAndScale(IValue overflow, IntegerValueBase intAugend, IntegerValueBase intAddend) {
            var s = intAugend.AsBigInteger + intAddend.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal IValue AndAndScale(IValue overflow, IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            var s = firstOperator.AsBigInteger & secondOperator.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal IValue DivideAndScale(IValue overflow, IntegerValueBase dividend, IntegerValueBase divisor) {
            var s = dividend.AsBigInteger / divisor.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal IValue ModuloAndScale(IValue overflow, IntegerValueBase dividend, IntegerValueBase divisor) {
            var s = dividend.AsBigInteger % divisor.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal IValue MultiplyAndScale(IValue overflow, IntegerValueBase multiplicand, IntegerValueBase multiplier) {
            var s = multiplicand.AsBigInteger * multiplier.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal IValue Negate(IValue overflow, IntegerValueBase intNumber) {
            var s = -intNumber.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal static IValue Not(IntegerValueBase intNumber)
            => intNumber.InvertBits();

        internal IValue OrAndScale(IValue overflow, IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            var s = firstOperator.AsBigInteger | secondOperator.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal IValue ShlAndScale(IValue overflow, IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            BigInteger s;
            var k1 = (firstOperator.TypeDefinition as IIntegralType ?? throw new InvalidOperationException()).Kind;

            if (k1 == IntegralTypeKind.Int64 || k1 == IntegralTypeKind.UInt64)
                s = new BigInteger(firstOperator.SignedValue << (int)secondOperator.SignedValue);
            else
                s = new BigInteger((int)firstOperator.SignedValue << (int)secondOperator.SignedValue);

            return ToIntValue(overflow, s);
        }

        internal IValue ShrAndScale(IValue overflow, IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            BigInteger s;
            var k1 = (firstOperator.TypeDefinition as IIntegralType ?? throw new InvalidOperationException()).Kind;

            if (k1 == IntegralTypeKind.Int64 || k1 == IntegralTypeKind.UInt64)
                s = new BigInteger((ulong)firstOperator.SignedValue >> (int)secondOperator.SignedValue);
            else
                s = new BigInteger((uint)firstOperator.SignedValue >> (int)secondOperator.SignedValue);

            return ToIntValue(overflow, s);
        }

        internal IValue SubtractAndScale(IValue overflow, IntegerValueBase intMinuend, IntegerValueBase intSubtrahend) {
            var s = intMinuend.AsBigInteger - intSubtrahend.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal IValue XorAndScale(IValue overflow, IntegerValueBase firstInt, IntegerValueBase secondInt) {
            var s = firstInt.AsBigInteger ^ secondInt.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(sbyte number)
            => new ShortIntValue(SystemUnit.ShortIntType, number);

        /// <summary>
        ///     convert a byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(byte number) {
            if (number < 128)
                return new ShortIntValue(SystemUnit.ShortIntType, (sbyte)number);

            return new ByteValue(SystemUnit.ByteType, number);
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(int number) {
            if (number < -32768)
                return new IntegerValue(SystemUnit.IntegerType, number);
            else if (number < -128)
                return new SmallIntValue(SystemUnit.SmallIntType, (short)number);
            else if (number < 128)
                return new ShortIntValue(SystemUnit.ShortIntType, (sbyte)number);
            else if (number < 256)
                return new ByteValue(SystemUnit.ByteType, (byte)number);
            else if (number < 32768)
                return new SmallIntValue(SystemUnit.SmallIntType, (short)number);
            else if (number < 65536)
                return new WordValue(SystemUnit.WordType, (ushort)number);
            else
                return new IntegerValue(SystemUnit.IntegerType, number);
        }

        /// <summary>
        ///     convert a unsigned int a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(uint number) {
            if (number < 128)
                return new ShortIntValue(SystemUnit.ShortIntType, (sbyte)number);
            else if (number < 256)
                return new ByteValue(SystemUnit.ByteType, (byte)number);
            else if (number < 32768)
                return new SmallIntValue(SystemUnit.SmallIntType, (short)number);
            else if (number < 65536)
                return new WordValue(SystemUnit.WordType, (ushort)number);
            else if (number < 2147483648)
                return new IntegerValue(SystemUnit.IntegerType, (int)number);

            return new CardinalValue(SystemUnit.CardinalType, number);
        }

        /// <summary>
        ///     convert a long to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(long number) {
            if (number < -2147483648)
                return new Int64Value(SystemUnit.Int64Type, number);
            else if (number < -32768)
                return new IntegerValue(SystemUnit.IntegerType, (int)number);
            else if (number < -128)
                return new SmallIntValue(SystemUnit.SmallIntType, (short)number);
            else if (number < 128)
                return new ShortIntValue(SystemUnit.ShortIntType, (sbyte)number);
            else if (number < 256)
                return new ByteValue(SystemUnit.ByteType, (byte)number);
            else if (number < 32768)
                return new SmallIntValue(SystemUnit.SmallIntType, (short)number);
            else if (number < 65536)
                return new WordValue(SystemUnit.WordType, (ushort)number);
            else if (number < 2147483648)
                return new IntegerValue(SystemUnit.IntegerType, (int)number);
            else if (number < 4294967296)
                return new CardinalValue(SystemUnit.CardinalType, (uint)number);
            else
                return new Int64Value(SystemUnit.Int64Type, number);
        }

        /// <summary>
        ///     convert a unsigned long to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(ulong number) {
            if (number < 128)
                return new ShortIntValue(SystemUnit.ShortIntType, (sbyte)number);
            else if (number < 256)
                return new ByteValue(SystemUnit.ByteType, (byte)number);
            else if (number < 32768)
                return new SmallIntValue(SystemUnit.SmallIntType, (short)number);
            else if (number < 65536)
                return new WordValue(SystemUnit.WordType, (ushort)number);
            else if (number < 2147483648)
                return new IntegerValue(SystemUnit.IntegerType, (int)number);
            else if (number < 4294967296)
                return new CardinalValue(SystemUnit.CardinalType, (uint)number);
            else if (number < 9223372036854775808)
                return new Int64Value(SystemUnit.Int64Type, (long)number);

            return new UInt64Value(SystemUnit.UInt64Type, number);
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(short number) {
            if (number < -128)
                return new SmallIntValue(SystemUnit.SmallIntType, number);
            else if (number < 128)
                return new ShortIntValue(SystemUnit.ShortIntType, (sbyte)number);
            else if (number < 256)
                return new ByteValue(SystemUnit.ByteType, (byte)number);

            return new SmallIntValue(SystemUnit.SmallIntType, number);
        }

        /// <summary>
        ///     convert a byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public IValue ToScaledIntegerValue(ushort number) {
            if (number < 128)
                return new ShortIntValue(SystemUnit.ShortIntType, (sbyte)number);
            else if (number < 256)
                return new ByteValue(SystemUnit.ByteType, (byte)number);
            else if (number < 32768)
                return new SmallIntValue(SystemUnit.SmallIntType, (short)number);

            return new WordValue(SystemUnit.WordType, number);
        }

        internal static bool GreaterThen(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger > secondInt.AsBigInteger;

        internal static bool GreaterThenEqual(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger >= secondInt.AsBigInteger;

        internal static bool Equal(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger == secondInt.AsBigInteger;

        internal static bool LessThenEqual(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger <= secondInt.AsBigInteger;

        internal static bool NotEqual(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger != secondInt.AsBigInteger;

        internal static bool LessThen(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger < secondInt.AsBigInteger;

        /// <summary>
        ///     invert all bits of this integer
        /// </summary>
        public abstract IValue InvertBits();

        /// <summary>
        ///     increment value
        /// </summary>
        /// <param name="overflow"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValue Increment(IValue overflow, IntegerValueBase value)
            => ToIntValue(overflow, value.AsBigInteger + BigInteger.One);

        /// <summary>
        ///     absolute value
        /// </summary>
        /// <param name="overflow"></param>
        /// <param name="integerValue"></param>
        /// <returns></returns>
        public IValue AbsoluteValue(IValue overflow, IntegerValueBase integerValue) {
            if (integerValue.IsNegative)
                return ToIntValue(overflow, BigInteger.Abs(integerValue.AsBigInteger));
            return integerValue;
        }

        /// <summary>
        ///     <c>chr</c> function
        /// </summary>
        /// <param name="integerValue"></param>
        /// <returns></returns>
        public IValue ChrValue(IntegerValueBase integerValue) {
            var b = integerValue.AsBigInteger;
            var value = b.ToByteArray();
            var negative = b < 0;
            var result = default(ushort);

            if (value.Length == 1 && !negative)
                result = value[0];
            else if (value.Length == 1 && negative)
                result = (ushort)(value[0] | (0xff << 8));
            else if (value.Length == 2)
                result = (ushort)(value[0] | ((value[1]) << 8));
            else if (value.Length >= 2)
                result = ushort.MaxValue;

            return new WideCharValue(SystemUnit.WideCharType, (char)result);
        }

        /// <summary>
        ///     hi value
        /// </summary>
        /// <param name="integerValue"></param>
        /// <returns></returns>
        public IValue HiValue(IntegerValueBase integerValue) {
            var b = integerValue.AsBigInteger;
            var value = b.ToByteArray();

            if (value.Length <= 1)
                return new ByteValue(SystemUnit.ByteType, 0);

            return new ByteValue(SystemUnit.ByteType, value[1]);

        }

        /// <summary>
        ///     hi value
        /// </summary>
        /// <param name="integerValue"></param>
        /// <returns></returns>
        public IValue LoValue(IntegerValueBase integerValue) {
            var b = integerValue.AsBigInteger;
            var value = b.ToByteArray();

            if (value.Length < 1)
                return new ByteValue(SystemUnit.ByteType, 0);

            return new ByteValue(SystemUnit.ByteType, value[0]);
        }

        /// <summary>
        ///     get the ordinal value of this value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public abstract IValue GetOrdinalValue(ITypeRegistry types);

        /// <summary>
        ///     swap routine
        /// </summary>
        /// <param name="overflow"></param>
        /// <param name="integerValue"></param>
        /// <param name="invalid"></param>
        /// <returns></returns>
        public IValue Swap(IValue overflow, IValue invalid, IntegerValueBase integerValue) {
            var b = integerValue.AsBigInteger;
            var value = b.ToByteArray();
            var typeDef = integerValue.TypeDefinition;

            if (!(typeDef is IIntegralType))
                return invalid;

            if (typeDef.TypeSizeInBytes < 2)
                return new ShortIntValue(SystemUnit.ShortIntType, 0);

            if (value.Length < 2)
                return new ShortIntValue(SystemUnit.ShortIntType, 0);

            var s = value[0];
            value[0] = value[1];
            value[1] = s;
            b = new BigInteger(value);
            return ToIntValue(overflow, b);

        }
    }
}
