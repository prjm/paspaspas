using System;
using System.Collections.Immutable;
using System.Linq;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     set values
    /// </summary>
    public class SetValue : RuntimeValueBase, IEquatable<SetValue> {

        /// <summary>
        ///     create a new set value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        public SetValue(int typeId, ImmutableArray<ITypeReference> values) : base(typeId)
            => Values = values.ToImmutableHashSet();

        /// <summary>
        ///     set values
        /// </summary>
        public IImmutableSet<ITypeReference> Values { get; }

        /// <summary>
        ///     internal type format
        /// </summary>
        public override string InternalTypeFormat
            => $"set of {TypeId} [({string.Join(", ", Values)})]";

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.SetType;

        /// <summary>
        ///     compare to another set value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SetValue other)
            => //
            other != default &&
            other.TypeId == TypeId &&
            Values.ToHashSet().SetEquals(other.Values);

        /// <summary>
        ///     compare for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => Equals(obj as SetValue);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;

            unchecked {

                result = result * 31 + TypeId;

                foreach (var value in Values)
                    result = result * 31 + value.GetHashCode();

                return result;
            }
        }
    }
}