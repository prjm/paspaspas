using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.FloatValues {

    /// <summary>
    ///     extended value
    /// </summary>
    public class ExtendedValue : FloatValueBase {

        private ExtF80 value;

        /// <summary>
        ///     create a new extend value
        /// </summary>
        /// <param name="extended"></param>
        public ExtendedValue(in ExtF80 extended)
            => value = extended;

        /// <summary>
        ///     test if the value is negative
        /// </summary>
        public override bool IsNegative
            => value.IsNegative;

        /// <summary>
        ///     get the extended value
        /// </summary>
        public override ExtF80 AsExtended
            => value;

        /// <summary>
        ///     type id: extended
        /// </summary>
        public override int TypeId
            => KnownTypeIds.Extended;

        /// <summary>
        ///     common type kind: real type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.RealType;

        /// <summary>
        ///     compare equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is IRealNumberValue floatValue)
                return floatValue.AsExtended == value;
            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => value.GetHashCode();

        /// <summary>
        ///     format this floating point value as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => value.ToString();
    }
}
