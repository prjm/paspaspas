using System;
using System.Collections.Immutable;
using System.Linq;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     set values
    /// </summary>
    internal class SetValue : RuntimeValueBase {

        /// <summary>
        ///     create a new set value
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="values"></param>
        public SetValue(ITypeDefinition typeDef, ImmutableArray<IValue> values) : base(typeDef)
            => Values = values.ToImmutableHashSet();

        /// <summary>
        ///     set values
        /// </summary>
        public IImmutableSet<IValue> Values { get; }

        /// <summary>
        ///     test if the set is empty
        /// </summary>
        public bool IsEmpty
            => Values.Count == 0;

        /// <summary>
        ///     compare to another set value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IValue? other) {

            if (!(other is SetValue sv))
                return false;

            var vals = Values.ToHashSet();
            var otherValues = sv.Values.ToHashSet();

            return vals.SetEquals(otherValues);
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var hashCode = new HashCode();
            hashCode.Add(TypeDefinition);
            foreach (var value in Values)
                hashCode.Add(value);
            return hashCode.ToHashCode();
        }

        /// <summary>
        ///     compare for equal values
        /// </summary>
        /// <param name="leftSet"></param>
        /// <param name="rightSet"></param>
        /// <returns></returns>
        internal static bool Equal(SetValue leftSet, SetValue rightSet) {
            foreach (var value in leftSet.Values)
                if (!rightSet.Values.Contains(value))
                    return false;

            foreach (var value in rightSet.Values)
                if (!leftSet.Values.Contains(value))
                    return false;

            return true;
        }

        internal static bool IsSuperset(SetValue leftSet, SetValue rightSet) {
            foreach (var value in rightSet.Values)
                if (!leftSet.Values.Contains(value))
                    return false;

            return true;

        }

        internal static bool IsSubset(SetValue leftSet, SetValue rightSet) {
            foreach (var value in leftSet.Values)
                if (!rightSet.Values.Contains(value))
                    return false;

            return true;
        }

        public override string GetValueString()
            => "[" + string.Join(",", Values.Select(t => t.ToValueString())) + "]";
    }
}