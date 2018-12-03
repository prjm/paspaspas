using PasPasPas.Globals.Runtime;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.FloatValues {

    /// <summary>
    ///     real number value factory and operations
    /// </summary>
    public class RealNumberOperations : IRealNumberOperations {

        /// <summary>
        ///     create a new real number operations helper
        /// </summary>
        /// <param name="booleans">boolean operations</param>
        public RealNumberOperations(IBooleanOperations booleans)
            => Booleans = booleans;

        /// <summary>
        ///     boolean operations
        /// </summary>
        public IBooleanOperations Booleans { get; }

        /// <summary>
        ///     get a constant real value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference ToExtendedValue(in ExtF80 value)
            => new ExtendedValue(value);


        /// <summary>
        ///     invalid real number
        /// </summary>
        public ITypeReference Invalid { get; }
            = new SpecialValue(SpecialConstantKind.InvalidReal);

        /// <summary>
        ///     floating point addition
        /// </summary>*
        /// <param name="augend"></param>
        /// <param name="addend"></param>
        /// <returns></returns>
        public ITypeReference Add(ITypeReference augend, ITypeReference addend) {
            if (augend is INumericalValue first && addend is INumericalValue second)
                return FloatValueBase.Add(first, second);

            return Invalid;
        }

        /// <summary>
        ///     floating point subtraction
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns></returns>
        public ITypeReference Subtract(ITypeReference minuend, ITypeReference subtrahend) {
            if (minuend is INumericalValue first && subtrahend is INumericalValue second)
                return FloatValueBase.Subtract(first, second);

            return Invalid;
        }

        /// <summary>
        ///     floating point multiplication
        /// </summary>
        /// <param name="multiplicand"></param>
        /// <param name="intMultiplier"></param>
        /// <returns></returns>
        public ITypeReference Multiply(ITypeReference multiplicand, ITypeReference intMultiplier) {
            if (multiplicand is INumericalValue first && intMultiplier is INumericalValue second)
                return FloatValueBase.Multiply(first, second);

            return Invalid;
        }

        /// <summary>
        ///     floating point division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public ITypeReference Divide(ITypeReference dividend, ITypeReference divisor) {
            if (dividend is INumericalValue numberDividend && divisor is INumericalValue numberDivisor)
                if (numberDivisor.AsExtended == 0)
                    return new SpecialValue(SpecialConstantKind.DivisionByZero);
                else
                    return FloatValueBase.Divide(numberDividend, numberDivisor);

            return Invalid;
        }

        /// <summary>
        ///     negate a floating point value
        /// </summary>
        /// <param name="number">value to negate</param>
        /// <returns></returns>
        public ITypeReference Negate(ITypeReference number) {
            if (number is INumericalValue floatValue)
                return FloatValueBase.Negate(floatValue);
            return Invalid;
        }

        /// <summary>
        ///     absolute value of a floating point value
        /// </summary>
        /// <param name="typeReference">value to negate</param>
        /// <returns></returns>
        public ITypeReference Abs(ITypeReference typeReference) {
            if (typeReference is INumericalValue floatValue)
                return FloatValueBase.Abs(floatValue);
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
        ///     check for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference Equal(ITypeReference left, ITypeReference right) {
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
        public ITypeReference NotEquals(ITypeReference left, ITypeReference right) {
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
        public ITypeReference LessThen(ITypeReference left, ITypeReference right) {
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
        public ITypeReference GreaterThenEqual(ITypeReference left, ITypeReference right) {
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
        public ITypeReference GreaterThen(ITypeReference left, ITypeReference right) {
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
        public ITypeReference LessThenOrEqual(ITypeReference left, ITypeReference right) {
            if (left is INumericalValue floatLeft && right is INumericalValue floatRight)
                return Booleans.ToBoolean(FloatValueBase.LessThenOrEqual(floatLeft, floatRight));
            else
                return Invalid;
        }

    }
}
