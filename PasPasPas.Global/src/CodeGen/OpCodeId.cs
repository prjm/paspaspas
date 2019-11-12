namespace PasPasPas.Globals.CodeGen {

    /// <summary>
    ///     op code id
    /// </summary>
    public enum OpCodeId : byte {


        /// <summary>
        ///     unknown operand code
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     <c>call</c> operand code
        /// </summary>
        Call = 1,
    }


    /// <summary>
    ///     op code helper
    /// </summary>
    public static class OpCodeHelper {

        /// <summary>
        ///     convert op code to byte value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this OpCodeId value)
            => (byte)value;

        /// <summary>
        ///     convert byte to op code value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static OpCodeId ToOpCodeId(this byte value)
            => (OpCodeId)value;

    }

}