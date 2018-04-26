using System;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Boolean {

    /// <summary>
    ///     base class for boolean values
    /// </summary>
    public abstract class BooleanValueBase : IBooleanValue {

        /// <summary>
        ///     get the type id
        /// </summary>
        public abstract int TypeId { get; }

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public abstract bool AsBoolean { get; }

        /// <summary>
        ///     always <c>true</c> for boolean constant values
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     format this number as string
        /// </summary>
        /// <returns>number as string</returns>
        public abstract override string ToString();

        internal static IValue And(IBooleanValue boolean1, IBooleanValue boolean2)
            => new BooleanValue(boolean1.AsBoolean && boolean2.AsBoolean);

        internal static IValue Not(IBooleanValue boolean)
            => new BooleanValue(!boolean.AsBoolean);

        internal static IValue Or(IBooleanValue boolean1, IBooleanValue boolean2)
            => new BooleanValue(boolean1.AsBoolean || boolean2.AsBoolean);

        internal static IValue Xor(IBooleanValue boolean1, IBooleanValue boolean2)
            => new BooleanValue(boolean1.AsBoolean ^ boolean2.AsBoolean);

        internal static IValue Equal(IBooleanValue boolean1, IBooleanValue boolean2)
            => new BooleanValue(boolean1.AsBoolean == boolean2.AsBoolean);

        internal static IValue GreaterThen(IBooleanValue boolean1, IBooleanValue boolean2)
            => new BooleanValue(boolean1.AsBoolean && (!boolean2.AsBoolean));

        internal static IValue LessThenOrEqual(IBooleanValue boolean1, IBooleanValue boolean2)
            => new BooleanValue(((!boolean1.AsBoolean) && boolean2.AsBoolean) || (boolean1.AsBoolean == boolean2.AsBoolean));

        internal static IValue NotEquals(IBooleanValue boolean1, IBooleanValue boolean2)
            => new BooleanValue(boolean1.AsBoolean != boolean2.AsBoolean);

        internal static IValue GreaterThenEqual(IBooleanValue boolean1, IBooleanValue boolean2)
            => new BooleanValue((boolean1.AsBoolean && (!boolean2.AsBoolean)) || (boolean1.AsBoolean == boolean2.AsBoolean));

        internal static IValue LessThen(IBooleanValue boolean1, IBooleanValue boolean2)
            => new BooleanValue((!boolean1.AsBoolean) && boolean2.AsBoolean);
    }
}
