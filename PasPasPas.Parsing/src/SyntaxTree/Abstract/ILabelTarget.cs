namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     labelled statement
    /// </summary>
    public interface ILabelTarget {

        /// <summary>
        ///     label name
        /// </summary>
        SymbolName LabelName { get; set; }
    }
}
