using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     special constant values
    /// </summary>
    internal class ErrorValue : RuntimeValueBase {

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
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IValue? other)
            => other is ErrorValue e && e.Kind == Kind;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked(17 + 31 * (int)Kind);

        public override string GetValueString()
            => "*** " + Kind.ToString();
    }
}