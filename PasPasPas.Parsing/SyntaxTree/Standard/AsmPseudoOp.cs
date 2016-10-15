namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     pseudo x64 operation
    /// </summary>
    public class AsmPseudoOp : SyntaxPartBase {

        /// <summary>
        ///     operation kind
        /// </summary>
        public Identifier Kind { get; set; }

        /// <summary>
        ///     skip stack frames
        /// </summary>
        public bool NoFrame { get; set; }

        /// <summary>
        ///     number of parameters
        /// </summary>
        public StandardInteger NumberOfParams { get; set; }

        /// <summary>
        ///     params pseudo op
        /// </summary>
        public bool ParamsOperation { get; set; }

        /// <summary>
        ///     pushenv pseudo op
        /// </summary>
        public bool PushEnvOperation { get; set; }

        /// <summary>
        ///     register name
        /// </summary>
        public Identifier Register { get; set; }

        /// <summary>
        ///     savenv pseudo op
        /// </summary>
        public bool SaveEnvOperation { get; set; }
    }
}
