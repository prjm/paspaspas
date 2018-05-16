namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for statements
    /// </summary>
    public abstract class StatementBase : AbstractSyntaxPartBase {


        /// <summary>
        ///     labeled
        /// </summary>
        public SymbolName LabelName { get; set; }

    }
}
