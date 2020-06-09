using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.IntValues;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     constant enumerated value
    /// </summary>
    internal class EnumeratedValue : IntegerValueBase, IIntegerValue, IEnumeratedValue {

        /// <summary>
        ///     create a new enumerated value
        /// </summary>
        /// <param name="typeDef">base type id</param>
        /// <param name="value">constant value</param>
        /// <param name="name">value name</param>
        public EnumeratedValue(ITypeDefinition typeDef, IIntegerValue value, string name) : base(typeDef, ((IIntegralType)value.TypeDefinition).Kind) {
            Value = value;
            Name = name;
        }


        /// <summary>
        ///     value name
        /// </summary>
        public string Name { get; }


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
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked(17 + 23 * TypeDefinition.GetHashCode() + 11 * Value.GetHashCode());

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

        public override string GetValueString() {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            else
                return Value.ToValueString();
        }

        public override bool Equals(IValue? other)
           => other is EnumeratedValue v && v.Value == Value;
    }
}