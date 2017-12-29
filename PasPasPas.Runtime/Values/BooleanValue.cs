using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Typings.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     class for boolean values
    /// </summary>
    public class BooleanValue : ValueBase {

        private byte[] data
            = new byte[1];

        /// <summary>
        ///     creates a new boolean value
        /// </summary>
        /// <param name="value"></param>
        public BooleanValue(bool value)
            => data[0] = value ? (byte)1 : (byte)0;

        /// <summary>
        ///     boolean data
        /// </summary>
        public override byte[] Data
            => data;

        /// <summary>
        ///     boolean type id
        /// </summary>
        public override int TypeId
            => TypeIds.BooleanType;

        public override bool Equals(object obj) {
            if (obj is BooleanValue b) {
                return b.data[0] == data[0];
            }

            return false;
        }

        public override int GetHashCode()
            => 17 + data[0];

        /// <summary>
        ///     convert
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => data[0] == 0 ? "false" : "true";
    }
}
