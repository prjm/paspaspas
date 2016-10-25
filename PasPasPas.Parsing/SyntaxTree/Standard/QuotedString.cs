namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     Quoted string
    /// </summary>
    public class QuotedString : SyntaxPartBase {

        /// <summary>
        ///     unquoted value
        /// </summary>
        public string UnquotedValue { get; set; }
            = string.Empty;

    }
}