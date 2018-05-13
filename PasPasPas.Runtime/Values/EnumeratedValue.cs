using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     constant enumerated value
    /// </summary>
    public class EnumeratedValue : IEnumeratedValue, IEquatable<IEnumeratedValue> {

        /// <summary>
        ///     create a new enumerated value
        /// </summary>
        /// <param name="enumTypeId">base type id</param>
        /// <param name="value">constant value</param>
        public EnumeratedValue(int enumTypeId, ITypeReference value) {
            TypeId = enumTypeId;
            Value = value;
        }

        /// <summary>
        ///     enumerated type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     <c>true</c> for this enumerated value
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.EnumerationType;

        /// <summary>
        ///     enumerated value
        /// </summary>
        public ITypeReference Value { get; }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is IEnumeratedValue enumValue)
                return Equals(enumValue);
            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => 17 + 23 * TypeId + 11 * Value.GetHashCode();

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IEnumeratedValue other)
            => (other.TypeId == TypeId) && (Value.Equals(other.Value));

        /// <summary>
        ///     convert this type to a short string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => StringUtils.Invariant($"Enum{TypeId}${Value}");

    }
}