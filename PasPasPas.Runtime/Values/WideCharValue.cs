using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Typings.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     wide char value
    /// </summary>
    public class WideCharValue : ValueBase {

        private byte[] data = new byte[2];

        /// <summary>
        ///     create a new wide char value
        /// </summary>
        /// <param name="singleChar"></param>
        public WideCharValue(char singleChar)
            => data = BitConverter.GetBytes(singleChar);

        /// <summary>
        ///     char
        /// </summary>
        public override byte[] Data
            => data;

        /// <summary>
        ///     common type id
        /// </summary>
        public override int TypeId
            => TypeIds.WideCharType;

        public override bool Equals(object obj) {
            if (obj is WideCharValue w) {
                return w.data[0] == data[0] && w.data[1] == data[1];
            }

            return false;
        }

        public override int GetHashCode()
            => 17 + 31 * data[0] + 31 * data[1];

        /// <summary>
        ///     convert the byte array to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => BitConverter.ToChar(data, 0).ToString();
    }
}
