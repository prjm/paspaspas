#nullable disable
using System;
using System.Collections.Immutable;
using System.Linq;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     constant array value
    /// </summary>
    public class ArrayValue : RuntimeValueBase, IEquatable<IArrayValue>, IArrayValue {

        /// <summary>
        ///     create a new array value
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="constantValues"></param>
        /// <param name="typeDef"></param>
        public ArrayValue(ITypeDefinition typeDef, ITypeDefinition baseType, ImmutableArray<IValue> constantValues) : base(typeDef) {
            BaseTypeDefinition = baseType;
            Values = constantValues;
        }

        /// <summary>
        ///     base type
        /// </summary>
        public ITypeDefinition BaseTypeDefinition { get; }

        /// <summary>
        ///     constant values
        /// </summary>
        public ImmutableArray<IValue> Values { get; }

        /// <summary>
        ///     compare to another array value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IArrayValue other)
            => other.BaseTypeDefinition.Equals(BaseType) && Enumerable.SequenceEqual(Values, other.Values);

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
                result = result * 31 + BaseType.GetHashCode();
                foreach (var item in Values)
                    result = result * 31 + item.GetHashCode();
                return result;
            }
        }

    }
}
