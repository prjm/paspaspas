namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     guid declaration
    /// </summary>
    public class InterfaceGuid : SyntaxPartBase {

        /// <summary>
        ///     guid for this interface
        /// </summary>
        public QuotedString Id { get; set; }

    }
}