using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     special constant values
    /// </summary>
    public class ErrorValue : RuntimeValueBase {

        /// <summary>
        ///     create a new special kind
        /// </summary>
        /// <param name="constantKind">constant kind</param>
        /// <param name="typeDef"></param>
        public ErrorValue(ITypeDefinition typeDef, SpecialConstantKind constantKind) : base(typeDef)
            => Kind = constantKind;

        /// <summary>
        ///     kind of this special value
        /// </summary>
        public SpecialConstantKind Kind { get; }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ErrorValue v) {
                return v.Kind == Kind;
            }

            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked(17 + 31 * (int)Kind);

    }
}