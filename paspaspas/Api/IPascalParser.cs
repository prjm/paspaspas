namespace PasPasPas.Api {

    /// <summary>
    ///     interface for a pascal parser
    /// </summary>
    interface IPascalParser {

        /// <summary>
        ///     tokenizer to use
        /// </summary>
        IPascalTokenizer BaseTokenizer { get; set; }

        /// <summary>
        ///     parse input
        /// </summary>
        ISyntaxPart Parse();
    }
}
