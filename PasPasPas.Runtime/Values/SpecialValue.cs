using System;
using PasPasPas.Infrastructure.Common;
using PasPasPas.Typings.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     special constant values
    /// </summary>
    public class SpecialValue : ValueBase {

        private SpecialConstantKind kind;

        /// <summary>
        ///     create a new special kind
        /// </summary>
        /// <param name="constantKind"></param>
        public SpecialValue(SpecialConstantKind constantKind)
            => kind = constantKind;

        /// <summary>
        ///     get the data
        /// </summary>
        public override byte[] Data {
            get {
                switch (kind) {
                    case SpecialConstantKind.IntegerOverflow:
                        return new byte[0];
                    default:
                        throw new ArgumentOutOfRangeException(nameof(kind));
                }
            }
        }

        /// <summary>
        ///     error type
        /// </summary>
        public override int TypeId
            => TypeIds.ErrorType;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is SpecialValue v) {
                return v.kind == kind;
            }

            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => 17 + 31 * (int)kind;

        /// <summary>
        ///     convert special value to string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            switch (kind) {
                case SpecialConstantKind.IntegerOverflow:
                    return "IO";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));

            }
        }
    }
}