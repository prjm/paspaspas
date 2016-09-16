namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     unit head
    /// </summary>
    public class UnitHead : SyntaxPartBase {

        /// <summary>
        ///     hinting directives
        /// </summary>
        public HintingInformationList Hint { get; set; }

        /// <summary>
        ///     unit name
        /// </summary>
        public NamespaceName UnitName { get; set; }

    }
}