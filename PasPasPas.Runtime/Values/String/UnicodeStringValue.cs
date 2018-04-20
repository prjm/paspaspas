﻿using System.Linq;
using System.Text;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.String {

    /// <summary>
    ///     Unicode string
    /// </summary>
    public class UnicodeStringValue : StringValueBase, IStringValue {

        private readonly string data;

        /// <summary>
        ///     get the unicode string value
        /// </summary>
        /// <param name="text"></param>
        public UnicodeStringValue(string text) {
            data = text;
        }

        /// <summary>
        ///     get the type id
        /// </summary>
        public override int TypeId
            => KnownTypeIds.UnicodeStringType;

        /// <summary>
        ///     get the string values
        /// </summary>
        public override string AsUnicodeString
            => data;

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

            for (var i = 0; i < data.Length; i++)
                result = result * 31 + data[i];

            return result;
        }

        /// <summary>
        ///     get the content of this string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => data;
    }
}