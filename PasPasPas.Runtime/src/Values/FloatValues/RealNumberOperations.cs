using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.FloatValues {

    /// <summary>
    ///     real number value factory and operations
    /// </summary>
    public class RealNumberOperations : IRealNumberOperations {

        /// <summary>
        ///     create a new real number operations helper
        /// </summary>
        /// <param name="typeProvider"></param>
        /// <param name="booleans">boolean operations</param>
        /// <param name="ints"></param>
        public RealNumberOperations(ITypeRegistryProvider typeProvider, IBooleanOperations booleans, IIntegerOperations ints) {
            Booleans = booleans;
            Ints = ints;
            provider = typeProvider;
            invalid = new Lazy<IValue>(() => new ErrorValue(provider.GetErrorType(), SpecialConstantKind.InvalidReal));
        }

        /// <summary>
        ///     boolean operations
        /// </summary>
        public IBooleanOperations Booleans { get; }

        /// <summary>
        ///     integers
        /// </summary>
        public IIntegerOperations Ints { get; }

        private readonly ITypeRegistryProvider provider;

        /// <summary>
        ///     get a constant real value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeDefinition"></param>
        /// <returns></returns>
        public IRealNumberValue ToExtendedValue(ITypeDefinition typeDefinition, in ExtF80 value)
            => new ExtendedValue(typeDefinition, value);

        /// <summary>
        ///     get a constant real value
        /// </summary>
        /// <param name="realValue"></param>
        /// <returns></returns>
        public IRealNumberValue ToExtendedValue(in ExtF80 realValue)
            => new ExtendedValue(provider.GetExtendedType(), realValue);


        private readonly Lazy<IValue> invalid;


        /// <summary>
        ///     invalid real number
        /// </summary>
        public IValue Invalid
            => invalid.Value;

        /// <summary>
        ///     rounding mode
        /// </summary>
        public RealNumberRoundingMode RoundingMode { get; set; }
            = RealNumberRoundingMode.ToNearest;

        /// <summary>
        ///     floating point addition
        /// </summary>*
        /// <param name="augend"></param>
        /// <param name="addend"></param>
        /// <returns></returns>
        public IValue Add(IValue augend, IValue addend) {
            if (augend is INumericalValue first && addend is INumericalValue second)
                return FloatValueBase.Add(provider.GetExtendedType(), first, second);

            return Invalid;
        }

        /// <summary>
        ///     floating point subtraction
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns></returns>
        public IValue Subtract(IValue minuend, IValue subtrahend) {
            if (minuend is INumericalValue first && subtrahend is INumericalValue second)
                return FloatValueBase.Subtract(provider.GetExtendedType(), first, second);

            return Invalid;
        }

        /// <summary>
        ///     floating point multiplication
        /// </summary>
        /// <param name="multiplicand"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public IValue Multiply(IValue multiplicand, IValue multiplier) {
            if (multiplicand is INumericalValue first && multiplier is INumericalValue second)
                return FloatValueBase.Multiply(provider.GetExtendedType(), first, second);

            return Invalid;
        }

        /// <summary>
        ///     floating point division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public IValue Divide(IValue dividend, IValue divisor) {
            if (dividend is INumericalValue numberDividend && divisor is INumericalValue numberDivisor)
                if (numberDivisor.AsExtended == 0)
                    return new ErrorValue(provider.GetErrorType(), SpecialConstantKind.DivisionByZero);
                else
                    return FloatValueBase.Divide(provider.GetExtendedType(), numberDividend, numberDivisor);

            return Invalid;
        }

        /// <summary>
        ///     negate a floating point value
        /// </summary>
        /// <param name="number">value to negate</param>
        /// <returns></returns>
        public IValue Negate(IValue number) {
            if (number is INumericalValue floatValue)
                return FloatValueBase.Negate(provider.GetExtendedType(), floatValue);
            return Invalid;
        }

        /// <summary>
        ///     absolute value of a floating point value
        /// </summary>
        /// <param name="typeReference">value to negate</param>
        /// <returns></returns>
        public IValue Abs(IValue typeReference) {
            if (typeReference is INumericalValue floatValue)
                return FloatValueBase.Abs(provider.GetExtendedType(), floatValue);
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
        ///     check for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue Equal(IValue left, IValue right) {
            if (left is INumericalValue floatLeft && right is INumericalValue floatRight)
                return Booleans.ToBoolean(FloatValueBase.Equal(floatLeft, floatRight));
            else
                return Invalid;
        }

        /// <summary>
        ///     check for inequality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue NotEquals(IValue left, IValue right) {
            if (left is INumericalValue floatLeft && right is INumericalValue floatRight)
                return Booleans.ToBoolean(FloatValueBase.NotEqual(floatLeft, floatRight));
            else
                return Invalid;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue LessThen(IValue left, IValue right) {
            if (left is INumericalValue floatLeft && right is INumericalValue floatRight)
                return Booleans.ToBoolean(FloatValueBase.LessThen(floatLeft, floatRight));
            else
                return Invalid;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue GreaterThenEqual(IValue left, IValue right) {
            if (left is INumericalValue floatLeft && right is INumericalValue floatRight)
                return Booleans.ToBoolean(FloatValueBase.GreaterThenEqual(floatLeft, floatRight));
            else
                return Invalid;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue GreaterThen(IValue left, IValue right) {
            if (left is INumericalValue floatLeft && right is INumericalValue floatRight)
                return Booleans.ToBoolean(FloatValueBase.GreaterThen(floatLeft, floatRight));
            else
                return Invalid;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue LessThenOrEqual(IValue left, IValue right) {
            if (left is INumericalValue floatLeft && right is INumericalValue floatRight)
                return Booleans.ToBoolean(FloatValueBase.LessThenOrEqual(floatLeft, floatRight));
            else
                return Invalid;
        }

        /// <summary>
        ///     round a floating point value
        /// </summary>
        /// <param name="realValue"></param>
        /// <returns></returns>
        public IValue Round(IValue realValue) {
            if (realValue is IIntegerValue)
                return realValue;

            if (!(realValue is IRealNumberValue value))
                return Invalid;

            var originalValue = value.AsExtended;
            ExtF80 roundedValue;

            switch (RoundingMode) {

                case RealNumberRoundingMode.Up:
                    roundedValue = originalValue.RoundToInt(SharpFloat.Globals.RoundingMode.Maximum, true);
                    break;

                case RealNumberRoundingMode.Down:
                    roundedValue = originalValue.RoundToInt(SharpFloat.Globals.RoundingMode.Minimum, true);
                    break;

                case RealNumberRoundingMode.ToNearest:
                    roundedValue = originalValue.RoundToInt(SharpFloat.Globals.RoundingMode.NearEven, true);
                    break;

                case RealNumberRoundingMode.Truncate:
                    var mode = originalValue.IsNegative ? SharpFloat.Globals.RoundingMode.Maximum : SharpFloat.Globals.RoundingMode.Minimum;
                    roundedValue = originalValue.RoundToInt(mode, true);
                    break;

                default:
                    return Invalid;

            }

            if (roundedValue >= long.MinValue && roundedValue <= long.MaxValue)
                return Ints.ToScaledIntegerValue((long)roundedValue);

            return Invalid;
        }

        /// <summary>
        ///     truncate a real number
        /// </summary>
        /// <param name="realNumberValue"></param>
        /// <returns></returns>
        public IValue Trunc(IRealNumberValue realNumberValue) {
            if (realNumberValue is IIntegerValue)
                return realNumberValue;

            if (!(realNumberValue is IRealNumberValue value))
                return Invalid;

            var originalValue = value.AsExtended;
            var mode = originalValue.IsNegative ? SharpFloat.Globals.RoundingMode.Maximum : SharpFloat.Globals.RoundingMode.Minimum;
            var roundedValue = originalValue.RoundToInt(mode, true);

            if (roundedValue >= long.MinValue && roundedValue <= long.MaxValue)
                return Ints.ToScaledIntegerValue((long)roundedValue);

            return Invalid;
        }
    }
}
