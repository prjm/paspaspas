#nullable disable
using System;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Dynamic {

    /// <summary>
    ///     undetermined value
    /// </summary>
    public class IndeterminedRuntimeValue : RuntimeValueBase, IEquatable<IndeterminedRuntimeValue> {

        /// <summary>
        ///     create a new undetermined runtime value
        /// </summary>
        /// <param name="typeDef">type id</param>
        public IndeterminedRuntimeValue(ITypeDefinition typeDef) : base(typeDef) { }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IndeterminedRuntimeValue other)
            => other != default && other.TypeDefinition.Equals(TypeDefinition);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => Equals(obj as IndeterminedRuntimeValue);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked(17 + 31 * TypeDefinition.GetHashCode());
    }
}