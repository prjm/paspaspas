using System;
using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.IntValues;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     constant enumerated value
    /// </summary>
    public class EnumeratedValue : IntegerValueBase, IIntegerValue, IEnumeratedValue, IEquatable<IEnumeratedValue> {

        /// <summary>
        ///     create a new enumerated value
        /// </summary>
        /// <param name="typeDef">base type id</param>
        /// <param name="value">constant value</param>
        public EnumeratedValue(ITypeDefinition typeDef, IIntegerValue value) : base(typeDef, ((IIntegralType)value.TypeDefinition).Kind)
            => Value = value;

        /// <summary>
        ///     enumerated value
        /// </summary>
        public IIntegerValue Value { get; }

        /// <summary>
        ///     test if this value negative
        /// </summary>
        public override bool IsNegative {
            get {
                if (Value is IntegerValueBase intValue)
                    return intValue.IsNegative;
                return false;
            }
        }

        /// <summary>
        ///     get the signed value
        /// </summary>
        public override long SignedValue {
            get {
                if (Value is IntegerValueBase intValue)
                    return intValue.SignedValue;
                return 0;
            }
        }

        /// <summary>
        ///     get the unsigned value
        /// </summary>
        public override ulong UnsignedValue {
            get {
                if (Value is IntegerValueBase intValue)
                    return intValue.UnsignedValue;
                return 0;
            }
        }

        /// <summary>
        ///     get the value as big integer
        /// </summary>
        public override BigInteger AsBigInteger {
            get {
                if (Value is IntegerValueBase intValue)
                    return intValue.AsBigInteger;
                return BigInteger.Zero;
            }
        }

        /// <summary>
        ///     invert bits
        /// </summary>
        /// <returns></returns>
        public override IValue InvertBits() {
            if (Value is IntegerValueBase intValue)
                return intValue.InvertBits();
            return Value;
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is IEnumeratedValue enumValue)
                return Equals(enumValue);
            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked(17 + 23 * TypeDefinition.GetHashCode() + 11 * Value.GetHashCode());

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IEnumeratedValue other)
            => other.TypeDefinition.Equals(TypeDefinition) && Value.Equals(other.Value);

        /// <summary>
        ///     get the ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types) {
            if (Value is IOrdinalValue value)
                return value.GetOrdinalValue(types);

            return types.Runtime.Integers.Invalid;
        }

    }
}