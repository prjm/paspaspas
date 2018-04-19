using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     special constant values
    /// </summary>
    public class SpecialValue : IValue {

        private SpecialConstantKind kind;

        /// <summary>
        ///     create a new special kind
        /// </summary>
        /// <param name="constantKind"></param>
        public SpecialValue(SpecialConstantKind constantKind)
            => kind = constantKind;

        /// <summary>
        ///     error type
        /// </summary>
        public int TypeId
            => KnownTypeIds.ErrorType;

        /// <summary>
        ///     kind of this special value
        /// </summary>
        public SpecialConstantKind Kind
            => kind;

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
                case SpecialConstantKind.InvalidInteger:
                    return "II";
                case SpecialConstantKind.InvalidReal:
                    return "IR";
                case SpecialConstantKind.DivisionByZero:
                    return "DZ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));

            }
        }
    }
}