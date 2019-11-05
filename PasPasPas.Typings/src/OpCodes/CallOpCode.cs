using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.OpCodes {

    /// <summary>
    ///     call opcode
    /// </summary>
    public class CallOpCode : OpCodeBase {

        /// <summary>
        ///     create a new call operation code
        /// </summary>
        /// <param name="callInfo"></param>
        public CallOpCode(IInvocationResult callInfo)
            => CallInfo = callInfo;

        /// <summary>
        ///     call information
        /// </summary>
        public IInvocationResult CallInfo { get; }

        /// <summary>
        ///     generate the opcode text
        /// </summary>
        public override string OpCodeText
            => $"call {CallInfo?.Routine?.Name}";

        /// <summary>
        ///     op code id
        /// </summary>
        public override byte Id =>
            OpCodeBase.CallId;
    }
}
