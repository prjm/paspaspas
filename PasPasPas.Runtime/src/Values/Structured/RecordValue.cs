using System;
using System.Collections.Immutable;
using System.Linq;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     constant record values
    /// </summary>
    public class RecordValue : ITypeReference, IEquatable<RecordValue> {

        /// <summary>
        ///     create a new record value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        public RecordValue(int typeId, ImmutableArray<ITypeReference> values) {
            TypeId = typeId;
            Values = values;
        }

        /// <summary>
        ///     record type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///    record value
        /// </summary>
        public ImmutableArray<ITypeReference> Values { get; }

        /// <summary>
        ///     internal type format
        /// </summary>
        public string InternalTypeFormat
            => $"record of {TypeId}";

        /// <summary>
        ///     format this value as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RecordValue other)
            => TypeId == other.TypeId && Enumerable.SequenceEqual(Values, other.Values);

        /// <summary>
        ///     constant value
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.ConstantValue;

        /// <summary>
        ///     record type
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.RecordType;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => Equals(obj as RecordValue);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            unchecked {
                var result = 17;
                result = result * 31 + TypeId;
                foreach (var value in Values)
                    result = result * 31 + value.GetHashCode();
                return result;
            }
        }
    }
}
