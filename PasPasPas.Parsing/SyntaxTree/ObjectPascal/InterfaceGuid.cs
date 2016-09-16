namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

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