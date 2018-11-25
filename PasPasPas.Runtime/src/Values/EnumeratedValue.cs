﻿using System;
using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Runtime.Values.IntValues;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     constant enumerated value
    /// </summary>
    public class EnumeratedValue : IntegerValueBase, IIntegerValue, IEnumeratedValue, IEquatable<IEnumeratedValue> {

        /// <summary>
        ///     create a new enumerated value
        /// </summary>
        /// <param name="enumTypeId">base type id</param>
        /// <param name="value">constant value</param>
        public EnumeratedValue(int enumTypeId, ITypeReference value) {
            TypeId = enumTypeId;
            Value = value;
        }

        /// <summary>
        ///     enumerated type id
        /// </summary>
        public override int TypeId { get; }

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.EnumerationType;

        /// <summary>
        ///     enumerated value
        /// </summary>
        public ITypeReference Value { get; }

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
        public override ITypeReference InvertBits() {
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
            => 17 + 23 * TypeId + 11 * Value.GetHashCode();

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IEnumeratedValue other)
            => (other.TypeId == TypeId) && (Value.Equals(other.Value));

        /// <summary>
        ///     convert this type to a short string
        /// </summary>
        /// <returns></returns>
        public override string InternalTypeFormat
            => Value.ToString();

    }
}