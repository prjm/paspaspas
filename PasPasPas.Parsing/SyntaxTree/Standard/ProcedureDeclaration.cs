namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure declaration
    /// </summary>
    public class ProcedureDeclaration : SyntaxPartBase {

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     function directives
        /// </summary>
        public FunctionDirectives Directives { get; set; }

        /// <summary>
        ///     procedure declaration heading
        /// </summary>
        public ProcedureDeclarationHeading Heading { get; set; }

        /// <summary>
        ///     procedure body
        /// </summary>
        public Block ProcedureBody { get; set; }

    }
}
