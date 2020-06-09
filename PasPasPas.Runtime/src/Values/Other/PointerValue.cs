using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     pointer value
    /// </summary>
    internal class PointerValue : RuntimeValueBase, IPointerValue {

        /// <summary>
        ///     create a new pointer value
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="value"></param>
        public PointerValue(ITypeDefinition typeDef, IValue value) : base(typeDef)
            => Value = value;

        /// <summary>
        ///     pointer value
        /// </summary>
        public IValue Value { get; }


        public override bool Equals(IValue? other)
            => other is PointerValue p && p.TypeDefinition.Equals(TypeDefinition) && p.Value.Equals(Value);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked(17 + 31 * TypeDefinition.GetHashCode() + 31 * Value.GetHashCode());

        public override string GetValueString()
            => "@" + Value.ToValueString();
    }
}
