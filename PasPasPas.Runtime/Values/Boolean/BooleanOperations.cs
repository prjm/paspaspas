using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Boolean {

    /// <summary>
    ///     boolean value factory and operations
    /// </summary>
    public class BooleanOperations : IBooleanOperations {

        /// <summary>
        ///     constant value: invalid boolean value
        /// </summary>
        public IValue Invalid { get; }
            = new SpecialValue(SpecialConstantKind.InvalidBool);

        /// <summary>
        ///     constant value: <c>true</c>
        /// </summary>
        public IValue TrueValue { get; }
            = new BooleanValue(true);

        /// <summary>
        ///     constant value: <c>false</c>
        /// </summary>
        public IValue FalseValue { get; }
            = new BooleanValue(false);

        /// <summary>
        ///     <c>and</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference And(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.And(boolean1, boolean2);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>==</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference Equal(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.Equal(boolean1, boolean2);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference GreaterThen(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.GreaterThen(boolean1, boolean2);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;=</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference GreaterThenEqual(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.GreaterThenEqual(boolean1, boolean2);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference LessThen(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.LessThen(boolean1, boolean2);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&lt;=</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference LessThenOrEqual(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.LessThenOrEqual(boolean1, boolean2);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>not</c> operation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference Not(ITypeReference value) {
            if (value is IBooleanValue boolean)
                return BooleanValueBase.Not(boolean);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>&gt;&lt;</c>
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference NotEquals(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.NotEquals(boolean1, boolean2);
            else
                return Invalid;
        }

        /// <summary>
        ///    <c>or</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference Or(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.Or(boolean1, boolean2);
            else
                return Invalid;
        }

        /// <summary>
        ///     <c>xor</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference Xor(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return BooleanValueBase.Xor(boolean1, boolean2);
            else
                return Invalid;

        }
    }
}
