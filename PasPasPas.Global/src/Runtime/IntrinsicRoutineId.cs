namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     intrinsic routine ids
    /// </summary>
    public enum IntrinsicRoutineId : byte {


        /// <summary>
        ///     unknown routine id / no intrinsic routine
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     <c>WriteLn</c> routine
        /// </summary>
        WriteLn = 1,

        /// <summary>
        ///     <c>sqr</c> routine
        /// </summary>
        Sqr = 2,

        /// <summary>
        ///     <c>trunc</c> routine
        /// </summary>
        Trunc = 3,

        /// <summary>
        ///     <c>sizeof</c> routine
        /// </summary>
        SizeOf = 4,

        /// <summary>
        ///     <c>round</c> routine
        /// </summary>
        Round = 5,

        /// <summary>
        ///     <c>ptr</c> routine
        /// </summary>
        PtrRoutine = 6,

        /// <summary>
        ///     <c>pred</c> routine
        /// </summary>
        Pred = 7,

        /// <summary>
        ///     <c>succ</c> routine
        /// </summary>
        Succ = 8,

        /// <summary>
        ///     <c>pi</c> routine
        /// </summary>
        Pi = 9,

        /// <summary>
        ///     <c>ord</c> routine
        /// </summary>
        Ord = 10,

        /// <summary>
        ///     <c>length</c> routine
        /// </summary>
        Length = 11,

        /// <summary>
        ///     <c>muldiv64</c> routine
        /// </summary>
        MulDiv64 = 12,

        /// <summary>
        ///     <c>odd</c> routine
        /// </summary>
        Odd = 13,

        /// <summary>
        ///     <c>swap</c> routine
        /// </summary>
        Swap = 14,

        /// <summary>
        ///     <c>high</c> routine
        /// </summary>
        High = 15,

        /// <summary>
        ///     <c>low</c> routine
        /// </summary>
        Low = 16,

        /// <summary>
        ///     <c>concat</c> routine
        /// </summary>
        Concat = 17,

        /// <summary>
        ///     <c>hi</c> routine
        /// </summary>
        Hi = 18,

        /// <summary>
        ///     <c>lo</c> routine
        /// </summary>
        Lo = 19,

        /// <summary>
        ///     <c>chr</c> routine
        /// </summary>
        Chr = 20,

        /// <summary>
        ///     <c>abs</c> routine
        /// </summary>
        Abs = 21,
    }

    /// <summary>
    ///     helper methods
    /// </summary>
    public static class IntrinsicRoutineIdHelper {

        /// <summary>
        ///     convert routine id to byte value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this IntrinsicRoutineId value)
            => (byte)value;

        /// <summary>
        ///     convert byte to routine id
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IntrinsicRoutineId ToIntrinsicRoutineId(this byte value)
            => (IntrinsicRoutineId)value;

    }

}
