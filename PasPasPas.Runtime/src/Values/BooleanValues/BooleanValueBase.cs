using System;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     base class for boolean values
    /// </summary>
    public abstract class BooleanValueBase : IBooleanValue, IEquatable<IBooleanValue> {

        /// <summary>
        ///     get the type id
        /// </summary>
        public abstract int TypeId { get; }

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public abstract bool AsBoolean { get; }

        /// <summary>
        ///     internal type format
        /// </summary>
        public abstract string InternalTypeFormat { get; }

        /// <summary>
        ///     always <c>true</c> for boolean constant values
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.ConstantValue;

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
            if (obj is IBooleanValue boolean)
                return Equals(boolean);

            return false;
        }

        /// <summary>
        ///     compute a simple hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => AsBoolean ? 1 : 0;

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

        /// <summary>
        ///     compare this value to another boolean value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBooleanValue other)
            => AsBoolean == other.AsBoolean;

        /// <summary>
        ///     format this boolean value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;
    }
}
