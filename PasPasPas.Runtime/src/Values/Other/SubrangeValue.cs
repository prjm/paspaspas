#nullable disable
using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     subrange value
    /// </summary>
    public class SubrangeValue : RuntimeValueBase, ISubrangeValue, IEquatable<SubrangeValue> {

        /// <summary>
        ///     create a new subrange value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="wrappedValue"></param>
        public SubrangeValue(ITypeDefinition typeId, IValue wrappedValue) : base(typeId)
            => WrappedValue = wrappedValue;

        /// <summary>
        ///     wrapped value
        /// </summary>
        public IValue WrappedValue { get; }

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
            => other.TypeDefinition.Equals(TypeDefinition) && other.WrappedValue.Equals(WrappedValue);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            unchecked {
                result += 31 * TypeDefinition.GetHashCode();
                result += 31 * WrappedValue.GetHashCode();
                return result;
            }
        }

        /// <summary>
        ///     get the ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public IValue GetOrdinalValue(ITypeRegistry types) {
            if (WrappedValue is IOrdinalValue ordinal)
                return ordinal.GetOrdinalValue(types);
            return types.Runtime.Integers.Invalid;
        }
    }
}
