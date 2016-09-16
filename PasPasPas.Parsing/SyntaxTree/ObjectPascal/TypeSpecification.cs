namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     a type specification
    /// </summary>
    public class TypeSpecification : SyntaxPartBase {

        /// <summary>
        ///     pointer type
        /// </summary>
        public PointerType PointerType { get; set; }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureType ProcedureType { get; set; }

        /// <summary>
        ///     simple type
        /// </summary>
        public SimpleType SimpleType { get; set; }

        /// <summary>
        ///     string type
        /// </summary>
        public StringType StringType { get; set; }

        /// <summary>
        ///     structured type
        /// </summary>
        public StructType StructuredType { get; set; }

    }
}