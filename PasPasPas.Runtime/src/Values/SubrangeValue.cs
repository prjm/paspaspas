using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     subrange value
    /// </summary>
    public class SubrangeValue : ISubrangeValue, IEquatable<SubrangeValue> {

        /// <summary>
        ///     create a new subrange value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        public SubrangeValue(int typeId, ITypeReference value) {
            TypeId = typeId;
            Value = value;
        }

        /// <summary>
        ///     wrapped value
        /// </summary>
        public ITypeReference Value { get; }

        /// <summary>
        ///     subrange type
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     format this type as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///     type format
        /// </summary>
        public string InternalTypeFormat
            => $"subrange {Value}";

        /// <summary>
        ///     type reference kind
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => Value.ReferenceKind;

        /// <summary>
        ///     subrange type
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.SubrangeType;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => Equals(obj as SubrangeValue);

        /// <summary>
        ///     test for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SubrangeValue other)
            => (other.TypeId == TypeId) && (other.Value.Equals(Value));

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            unchecked {
                result = result + 31 * TypeId;
                result = result + 31 * Value.GetHashCode();
                return result;
            }
        }

        /// <summary>
        ///     get the ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public ITypeReference GetOrdinalValue(ITypeRegistry types) {
            if (Value is IOrdinalValue ordinal)
                return ordinal.GetOrdinalValue(types);
            return types.Runtime.Types.MakeErrorTypeReference();
        }
    }
}
