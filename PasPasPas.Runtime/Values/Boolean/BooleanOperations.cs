using System;
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
        public IBooleanValue TrueValue { get; }
            = new BooleanValue(true);

        /// <summary>
        ///     constant value: <c>false</c>
        /// </summary>
        public IBooleanValue FalseValue { get; }
            = new BooleanValue(false);

        /// <summary>
        ///     boolean operations
        /// </summary>
        public IBooleanOperations Booleans
            => this;

        /// <summary>
        ///     convert a simple bool to a boolean value
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns>boolean constant value</returns>
        public IBooleanValue AsBoolean(bool value)
            => value ? TrueValue : FalseValue;

        /// <summary>
        ///     <c>and</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public ITypeReference And(ITypeReference value1, ITypeReference value2) {
            if (value1 is IBooleanValue boolean1 && value2 is IBooleanValue boolean2)
                return AsBoolean(BooleanValueBase.And(boolean1, boolean2));
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
                return AsBoolean(BooleanValueBase.Equal(boolean1, boolean2));
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
                return AsBoolean(BooleanValueBase.GreaterThen(boolean1, boolean2));
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
                return AsBoolean(BooleanValueBase.GreaterThenEqual(boolean1, boolean2));
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
                return AsBoolean(BooleanValueBase.LessThen(boolean1, boolean2));
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
                return AsBoolean(BooleanValueBase.LessThenOrEqual(boolean1, boolean2));
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
                return AsBoolean(BooleanValueBase.Not(boolean));
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
                return AsBoolean(BooleanValueBase.NotEquals(boolean1, boolean2));
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
                return AsBoolean(BooleanValueBase.Or(boolean1, boolean2));
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
                return AsBoolean(BooleanValueBase.Xor(boolean1, boolean2));
            else
                return Invalid;

        }
    }
}
