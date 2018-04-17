using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.String {

    /// <summary>
    ///     string calculator
    /// </summary>
    public class StringCalculator : IStringCalculator {

        private IValue invalidString
            = new SpecialValue(SpecialConstantKind.InvalidString);

        /// <summary>
        ///     concatenate two strings
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue Concat(IValue value1, IValue value2) {
            if (value1 is IStringValue string1 && value2 is IStringValue string2)
                return StringValueBase.Concat(string1, string2);
            else
                return invalidString;
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue Equal(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return StringValueBase.Equal(string1, string2);
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue GreaterThen(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return StringValueBase.GreaterThen(string1, string2);
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue GreaterThenEqual(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return StringValueBase.GreaterThenEqual(string1, string2);
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue LessThen(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return StringValueBase.LessThen(string1, string2);
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue LessThenOrEqual(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return StringValueBase.LessThenOrEqual(string1, string2);
            else
                return invalidString;

        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue NotEquals(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return StringValueBase.NotEquals(string1, string2);
            else
                return invalidString;
        }
    }
}
