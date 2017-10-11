namespace SampleRunner.Scenarios {

    /// <summary>
    ///     sample mode
    /// </summary>
    public enum SampleMode {

        /// <summary>
        ///     undefined mode
        /// </summary>
        Undefined,

        /// <summary>
        ///     read a file
        /// </summary>
        ReadFile,

        /// <summary>
        ///     tokenizer a file
        /// </summary>
        TokenizerFile,

        /// <summary>
        ///     tokenize a file in a buffer
        /// </summary>
        BufferedTokenizeFile,

        /// <summary>
        ///     parser a file
        /// </summary>
        ParseFile
    }
}
