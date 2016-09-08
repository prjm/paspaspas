namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     exported procedure heading for an interace section
    /// </summary>
    public class ExportedProcedureHeading : SyntaxPartBase {

        /// <summary>
        ///     function directives
        /// </summary>
        public FunctionDirectives Directives { get; set; }

        /// <summary>
        ///     heading kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     exported proc name
        /// </summary>
        public PascalIdentifier Name { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     result attributes
        /// </summary>
        public UserAttributes ResultAttributes { get; set; }

        /// <summary>
        ///     result types
        /// </summary>
        public TypeSpecification ResultType { get; set; }

    }

}
