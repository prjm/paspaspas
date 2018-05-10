using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.BooleanValues {

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
        ///     type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.BooleanType;

        /// <summary>
        ///     compare to another boolean value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (!(obj is IBooleanValue boolean))
                return false;

            return boolean.AsBoolean == AsBoolean;
        }

        /// <summary>
        ///     compute a simple hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return AsBoolean ? 1 : 0;
        }

        /// <summary>
        ///     format this number as string
        /// </summary>
        /// <returns>number as string</returns>
        public abstract override string ToString();

        internal static bool And(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean && boolean2.AsBoolean;

        internal static bool Not(IBooleanValue boolean)
            => !boolean.AsBoolean;

        internal static bool Or(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean || boolean2.AsBoolean;

        internal static bool Xor(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean ^ boolean2.AsBoolean;

        internal static bool Equal(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean == boolean2.AsBoolean;

        internal static bool GreaterThen(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean && (!boolean2.AsBoolean);

        internal static bool LessThenOrEqual(IBooleanValue boolean1, IBooleanValue boolean2)
            => ((!boolean1.AsBoolean) && boolean2.AsBoolean) || (boolean1.AsBoolean == boolean2.AsBoolean);

        internal static bool NotEquals(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean != boolean2.AsBoolean;

        internal static bool GreaterThenEqual(IBooleanValue boolean1, IBooleanValue boolean2)
            => (boolean1.AsBoolean && (!boolean2.AsBoolean)) || (boolean1.AsBoolean == boolean2.AsBoolean);

        internal static bool LessThen(IBooleanValue boolean1, IBooleanValue boolean2)
            => (!boolean1.AsBoolean) && boolean2.AsBoolean;
    }
}
