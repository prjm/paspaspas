using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     wide char (word) value
    /// </summary>
    public class WideCharValue : CharValueBase, IEquatable<WideCharValue> {

        /// <summary>
        ///     create a new wide char value
        /// </summary>
        /// <param name="character"></param>
        /// <param name="typeId">type id</param>
        public WideCharValue(int typeId, char character) {
            TypeId = typeId;
            Value = character;
        }

        /// <summary>
        ///     type id
        /// </summary>
        public override int TypeId { get; }

        /// <summary>
        ///     char value
        /// </summary>
        public override char AsWideChar
            => Value;

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.WideCharType;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => obj is WideCharValue charValue && charValue.Value == Value;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(WideCharValue other)
            => Value == other.Value;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => Value.GetHashCode();

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override ITypeReference GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);

        /// <summary>
        ///     char value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override ITypeReference CharAt(int index) {
            if (index < 0 || index >= 1)
                return new SpecialValue(SpecialConstantKind.InvalidChar);

            return this;
        }

        /// <summary>
        ///     convert this value to a string
        /// </summary>
        /// <returns></returns>
        public override string InternalTypeFormat
            => StringUtils.Invariant($"'{Value}'");

        /// <summary>
        ///     char value
        /// </summary>
        public char Value { get; }
    }
}
