using System;
using System.Collections.Immutable;
using System.Linq;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     constant record values
    /// </summary>
    internal class RecordValue : RuntimeValueBase {

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

        public override bool Equals(IValue? other)
            => other is RecordValue r && r.Values.SequenceEqual(Values);

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

        public override string GetValueString() {
            var recordType = TypeDefinition as IStructuredType ?? throw new InvalidOperationException(); ;
            using var sbv = TypeDefinition.DefiningUnit.TypeRegistry.StringBuilderPool.Borrow();
            var sb = sbv.Item;
            sb.Append("(");

            for (var i = 0; i < recordType.Fields.Count; i++) {
                if (i > 0)
                    sb.Append("; ");
                var field = recordType.Fields[i];
                sb.Append(field.Name);
                sb.Append(":");
                sb.Append(Values[i].ToValueString());
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
