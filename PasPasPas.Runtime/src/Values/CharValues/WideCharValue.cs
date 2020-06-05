﻿#nullable disable
using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Other;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     wide char (word) value
    /// </summary>
    public class WideCharValue : CharValueBase, IEquatable<WideCharValue> {

        /// <summary>
        ///     create a new wide char value
        /// </summary>
        /// <param name="character"></param>
        /// <param name="typeDef">type id</param>
        public WideCharValue(ITypeDefinition typeDef, char character) : base(typeDef, CharTypeKind.WideChar)
            => Value = character;

        /// <summary>
        ///     char value
        /// </summary>
        public override char AsWideChar
            => Value;

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
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);

        /// <summary>
        ///     char value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override IValue CharAt(int index) {
            if (index < 0 || index >= 1)
                return new ErrorValue(SystemUnit.ErrorType, SpecialConstantKind.InvalidChar);

            return this;
        }

        /// <summary>
        ///     char value
        /// </summary>
        public char Value { get; }
    }
}
