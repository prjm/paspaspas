using System;
using System.Collections.Immutable;
using System.Linq;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     constant record values
    /// </summary>
    public class RecordValue : RuntimeValueBase, IEquatable<RecordValue> {

        /// <summary>
        ///     create a new record value
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="values"></param>
        public RecordValue(ITypeDefinition typeDef, ImmutableArray<IValue> values) : base(typeDef)
            => Values = values;

        /// <summary>
        ///    record value
        /// </summary>
        public ImmutableArray<IValue> Values { get; }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RecordValue other)
            => other != default && TypeDefinition.Equals(other.TypeDefinition) && Enumerable.SequenceEqual(Values, other.Values);

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
                result = result * 31 + GetHashCode();
                foreach (var value in Values)
                    result = result * 31 + value.GetHashCode();
                return result;
            }
        }
    }
}
