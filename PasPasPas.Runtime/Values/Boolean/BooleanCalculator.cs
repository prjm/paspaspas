using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Boolean {

    /// <summary>
    ///     boolean calculator
    /// </summary>
    public class BooleanCalculator : IBooleanCalculator {

        private IValue invalidBool
            = new SpecialValue(SpecialConstantKind.InvalidBool);

        /// <summary>
        ///     <c>and</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue And(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.And(boolean1, boolean2);
            else
                return invalidBool;
        }

        /// <summary>
        ///     <c>==</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue Equal(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.Equal(boolean1, boolean2);
            else
                return invalidBool;
        }

        /// <summary>
        ///     <c>&gt;</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue GreaterThen(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.GreaterThen(boolean1, boolean2);
            else
                return invalidBool;
        }

        /// <summary>
        ///     <c>&gt;=</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue GreaterThenEqual(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.GreaterThenEqual(boolean1, boolean2);
            else
                return invalidBool;
        }

        /// <summary>
        ///     <c>&lt;</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue LessThen(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.LessThen(boolean1, boolean2);
            else
                return invalidBool;
        }

        /// <summary>
        ///     <c>&lt;=</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue LessThenOrEqual(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.LessThenOrEqual(boolean1, boolean2);
            else
                return invalidBool;
        }

        /// <summary>
        ///     <c>not</c> operation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValue Not(IValue value) {
            if (value is IBooleanValue boolean)
                return BooleanValueBase.Not(boolean);
            else
                return invalidBool;
        }

        /// <summary>
        ///     <c>&gt;&lt;</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue NotEquals(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.NotEquals(boolean1, boolean2);
            else
                return invalidBool;
        }

        /// <summary>
        ///    <c>or</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue Or(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.Or(boolean1, boolean2);
            else
                return invalidBool;
        }

        /// <summary>
        ///     <c>xor</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IValue Xor(IValue value1, IValue value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.Xor(boolean1, boolean2);
            else
                return invalidBool;

        }
    }
}
