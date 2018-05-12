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
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference Add(ITypeReference value1, ITypeReference value2) {
            if (value1 is INumericalValue first && value2 is INumericalValue second)
                return FloatValueBase.Add(first, second);

            return Invalid;
        }

        /// <summary>
        ///     floating point subtraction
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference Subtract(ITypeReference value1, ITypeReference value2) {
            if (value1 is INumericalValue first && value2 is INumericalValue second)
                return FloatValueBase.Subtract(first, second);

            return Invalid;
        }

        /// <summary>
        ///     floating point multiplication
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference Multiply(ITypeReference value1, ITypeReference value2) {
            if (value1 is INumericalValue first && value2 is INumericalValue second)
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
        /// <param name="value">value to negate</param>
        /// <returns></returns>
        public ITypeReference Negate(ITypeReference value) {
            if (value is INumericalValue floatValue)
                return FloatValueBase.Negate(floatValue);
            return Invalid;
        }

        /// <summary>
        ///     identity function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference Identity(ITypeReference value) {
            return value;
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference Equal(ITypeReference left, ITypeReference right) {
            if (left is INumericalValue floatLeft && right is INumericalValue floatRight)
                return Booleans.AsBoolean(FloatValueBase.Equal(floatLeft, floatRight));
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
                return Booleans.AsBoolean(FloatValueBase.NotEqual(floatLeft, floatRight));
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
                return Booleans.AsBoolean(FloatValueBase.LessThen(floatLeft, floatRight));
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
                return Booleans.AsBoolean(FloatValueBase.GreaterThenEqual(floatLeft, floatRight));
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
                return Booleans.AsBoolean(FloatValueBase.GreaterThen(floatLeft, floatRight));
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
                return Booleans.AsBoolean(FloatValueBase.LessThenOrEqual(floatLeft, floatRight));
            else
                return Invalid;
        }

    }
}
