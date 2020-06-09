using System.Collections.Immutable;
using System.Linq;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     constant array value
    /// </summary>
    internal class ArrayValue : RuntimeValueBase, IArrayValue {

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

        public override bool Equals(IValue? other)
            => other is ArrayValue r && r.Values.SequenceEqual(Values);

        public override string GetValueString()
            => "[" + string.Join(",", Values.Select(t => t.ToValueString())) + "]";
    }
}
