using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     pointer value
    /// </summary>
    public class PointerValue : IPointerValue, IEquatable<PointerValue> {

        /// <summary>
        ///     create a new pointer value
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="value"></param>
        public PointerValue(int baseType, IOldTypeReference value) {
            BaseType = baseType;
            Value = value;
        }

        /// <summary>
        ///     base type
        /// </summary>
        public int BaseType { get; }

        /// <summary>
        ///     pointer value
        /// </summary>
        public IOldTypeReference Value { get; }

        /// <summary>
        ///     pointer type
        /// </summary>
        public int TypeId
            => KnownTypeIds.GenericPointer;

        /// <summary>
        ///     internal type format
        /// </summary>
        public string InternalTypeFormat
            => "@" + Value.InternalTypeFormat;

        /// <summary>
        ///     reference kind
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => Value.ReferenceKind;

        /// <summary>
        ///     pointer type
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.PointerType;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PointerValue other)
            => TypeId == other.TypeId && Value.Equals(other.Value);

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
            => unchecked(17 + 31 * TypeId + 31 * Value.GetHashCode());
    }
}
