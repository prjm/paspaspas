using System;
using System.Collections.Immutable;
using System.Linq;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     constant array value
    /// </summary>
    public class ArrayValue : IEquatable<IArrayValue>, IArrayValue {

        /// <summary>
        ///     create a new array value
        /// </summary>
        /// <param name="baseTypeId"></param>
        /// <param name="constantValues"></param>
        /// <param name="typeId"></param>
        public ArrayValue(int typeId, int baseTypeId, ImmutableArray<ITypeReference> constantValues) {
            TypeId = typeId;
            BaseType = baseTypeId;
            Values = constantValues;
        }

        /// <summary>
        ///     base type
        /// </summary>
        public int BaseType { get; }

        /// <summary>
        ///     constant values
        /// </summary>
        public ImmutableArray<ITypeReference> Values { get; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.ConstantArrayType;

        /// <summary>
        ///     format this type
        /// </summary>
        public string InternalTypeFormat
            => $"array {TypeId} of {BaseType} [({string.Join(", ", Values)})]";

        /// <summary>
        ///     format this value as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///     compare to another array value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IArrayValue other)
            => other.BaseType == BaseType && Enumerable.SequenceEqual(Values, other.Values);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => obj is IArrayValue array ? Equals(array) : false;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            unchecked {
                var result = 17;
                result = result * 31 + BaseType;
                foreach (var item in Values)
                    result = result * 31 + item.GetHashCode();
                return result;
            }
        }

        /// <summary>
        ///     reference kind: constant
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.ConstantValue;
    }
}
