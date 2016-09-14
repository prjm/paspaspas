namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     procedure reference
    /// </summary>
    public class ProcedureReference : SyntaxPartBase {

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureTypeDefinition ProcedureType { get; set; }

    }
}