using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Runtime.Common {

    /// <summary>
    ///     helper class for byte array calculations
    /// </summary>
    public class ByteArrayCalculation {

        /// <summary>
        ///     create a new byte array calculation
        /// </summary>
        /// <param name="isNegative"></param>
        /// <param name="data"></param>
        /// <param name="overflow"></param>
        public ByteArrayCalculation(bool isNegative, byte[] data, bool overflow) {
            IsNegative = isNegative;
            Data = data;
            Overflow = overflow;
        }

        /// <summary>
        ///     <c>true</c> if overflow occurred
        /// </summary>
        public bool Overflow { get; set; }

        /// <summary>
        ///     data array
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        ///     <c>true</c> if this number should be interpreted as negative number
        /// </summary>
        public bool IsNegative { get; set; }
    }
}
