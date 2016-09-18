namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure declaration heading
    /// </summary>
    public class ProcedureDeclarationHeading : SyntaxPartBase {

        /// <summary>
        ///     procedure kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     procedure name
        /// </summary>
        public Identifier Name { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     result type
        /// </summary>
        public TypeSpecification ResultType { get; set; }

        /// <summary>
        ///     result attributes
        /// </summary>
        public UserAttributes ResultTypeAttributes { get; set; }

    }
}