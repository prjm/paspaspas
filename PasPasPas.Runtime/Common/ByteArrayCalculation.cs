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
        public ByteArrayCalculation(bool isNegative, byte[] data) {
            IsNegative = isNegative;
            Data = data;
            Overflow = false;
        }

        /// <summary>
        ///     create a new byte array calculation
        /// </summary>
        /// <param name="overflow"></param>
        public ByteArrayCalculation(bool overflow) {
            Overflow = overflow;
        }

        /// <summary>
        ///     <c>true</c> if overflow occurred
        /// </summary>
        public bool Overflow { get; private set; }

        /// <summary>
        ///     data array
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        ///     <c>true</c> if this number should be interpreted as negative number
        /// </summary>
        public bool IsNegative { get; private set; }
    }
}
