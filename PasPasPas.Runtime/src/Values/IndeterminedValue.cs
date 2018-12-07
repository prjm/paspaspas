using System;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     indetermined value
    /// </summary>
    public class IndeterminedRuntimeValue : ITypeReference, IEquatable<IndeterminedRuntimeValue> {

        /// <summary>
        ///     create a new indetermined runtime value
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <param name="typeKind">type kind</param>
        public IndeterminedRuntimeValue(int typeId, CommonTypeKind typeKind) {
            TypeId = typeId;
            TypeKind = typeKind;
        }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     fixed type kind
        /// </summary>
        public CommonTypeKind TypeKind { get; }

        /// <summary>
        ///     reference kind
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.DynamicValue;

        /// <summary>
        ///     short string for this runtime value
        /// </summary>
        /// <returns></returns>
        public string InternalTypeFormat
            => "*";

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IndeterminedRuntimeValue other)
            => other != default && other.TypeId == TypeId && other.TypeKind == TypeKind;

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
            => unchecked(17 + 31 * TypeId + 31 * (int)TypeKind);
    }
}