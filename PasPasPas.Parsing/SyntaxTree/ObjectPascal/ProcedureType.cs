namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     procedurual type
    /// </summary>
    public class ProcedureType : SyntaxPartBase {

        /// <summary>
        ///     procedure reference
        /// </summary>
        public ProcedureReference ProcedureReference { get; set; }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureTypeDefinition ProcedureRefType { get; set; }

    }
}