using PasPasPas.Globals.CodeGen;

namespace PasPasPas.Typings.OpCodes {

    /// <summary>
    ///     base class for op codes
    /// </summary>
    public abstract class OpCodeBase : IOpCode {

        /// <summary>
        /// get the opcode text
        /// </summary>
        public abstract string OpCodeText { get; }
    }
}