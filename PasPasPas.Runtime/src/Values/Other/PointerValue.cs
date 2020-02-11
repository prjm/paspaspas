using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     pointer value
    /// </summary>
    public class PointerValue : RuntimeValueBase, IPointerValue, IEquatable<PointerValue> {

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

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PointerValue other)
            => TypeDefinition.Equals(other.TypeDefinition) && Value.Equals(other.Value);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => Equals(obj as PointerValue);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked(17 + 31 * TypeDefinition.GetHashCode() + 31 * Value.GetHashCode());
    }
}
