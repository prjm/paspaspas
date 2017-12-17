using PasPasPas.Infrastructure.Common;

namespace PasPasPas.Runtime.Operators {

    /// <summary>
    ///     constant operations helper
    /// </summary>
    public class OperatorsOnConstants : IConstantOperations {

        /// <summary>
        ///     negate a value
        /// </summary>
        /// <param name="value">value to negate</param>
        /// <returns>negated value</returns>
        public object Negate(object value) {
            if (value is sbyte i8)
                return ToConstantInt(-i8);
            else if (value is byte ui8)
                return ToConstantInt(-ui8);
            else if (value is short i16)
                return ToConstantInt(-i16);
            else if (value is ushort ui16)
                return ToConstantInt(-ui16);
            else if (value is int i32)
                return ToConstantInt(-i32);
            else if (value is uint ui32)
                return ToConstantInt(-ui32);
            if (value is long i64)
                return ToConstantInt(-i64);
            if (value is ulong ui64 && ui64 <= 9223372036854775807)
                return ToConstantInt(-((long)ui64));
            if (value is double d)
                return -d;

            throw new InvalidArithmeticOperationException("Negate", value);
        }

        /// <summary>
        ///     convert an integral number to an internal literal
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public object ToConstantInt(long result) {
            if (result < -2147483648)
                return result;
            else if (result < -32768)
                return (int)result;
            else if (result < -128)
                return (short)result;
            else if (result < 128)
                return (sbyte)result;
            else if (result < 256)
                return (byte)result;
            else if (result < 32768)
                return (short)result;
            else if (result < 65536)
                return (ushort)result;
            else if (result < 2147483648)
                return (int)result;
            else if (result < 4294967296)
                return (uint)result;

            return result;
        }
    }
}