using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Typings.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     unicode string
    /// </summary>
    public class UnicodeStringValue : ValueBase {

        private byte[] data;

        /// <summary>
        ///     get the unicode string value
        /// </summary>
        /// <param name="text"></param>
        public UnicodeStringValue(string text) {
            data = Encoding.Unicode.GetBytes(text);
        }

        /// <summary>
        ///     string data
        /// </summary>
        public override byte[] Data
            => data;

        /// <summary>
        ///     get the type id
        /// </summary>
        public override int TypeId
            => TypeIds.UnicodeStringType;

        /// </summary>
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
            => Encoding.Unicode.GetString(data);
    }
}
