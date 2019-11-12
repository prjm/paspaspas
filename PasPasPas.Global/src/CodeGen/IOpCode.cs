namespace PasPasPas.Globals.CodeGen {

    /// <summary>
    ///     basic interface for operation codes
    /// </summary>
    public interface IOpCode {

        /// <summary>
        ///     textual representation
        /// </summary>
        string OpCodeText { get; }

        /// <summary>
        ///    op-code id
        /// </summary>
        OpCodeId Id { get; }
    }
}
