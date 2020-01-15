﻿using System.Linq;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.CharValues;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     Unicode string
    /// </summary>
    public class UnicodeStringValue : StringValueBase, IStringValue {

        private readonly string data;

        /// <summary>
        ///     get the Unicode string value
        /// </summary>
        /// <param name="text"></param>
        /// <param name="typeId">type id</param>
        public UnicodeStringValue(int typeId, string text) : base(typeId)
            => data = text;

        /// <summary>
        ///     get the string values
        /// </summary>
        public override string AsUnicodeString
            => data;

        /// <summary>
        ///     fixed type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.UnicodeStringType;

        /// <summary>test for equality</summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is UnicodeStringValue v) {
                return data.SequenceEqual(v.data);
            }

            return false;
        }

        /// <summary>
        ///     hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;

            unchecked {
                for (var i = 0; i < data.Length; i++)
                    result = result * 31 + data[i];
            }

            return result;
        }

        /// <summary>
        ///     convert this value to in internal string format
        /// </summary>
        public override string InternalTypeFormat
            => $"'{data}'";

        /// <summary>
        ///     number of characters
        /// </summary>
        public override int NumberOfCharElements
            => data.Length;

        /// <summary>
        ///     get the content of this string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///     char at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override IOldTypeReference CharAt(int index) {
            if (index < 0 || index >= data.Length)
                return new SpecialValue(SpecialConstantKind.InvalidChar);

            return new WideCharValue(KnownTypeIds.WideCharType, data[index]);
        }
    }
}
