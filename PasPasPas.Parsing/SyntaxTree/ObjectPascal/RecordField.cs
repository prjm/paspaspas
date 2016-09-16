namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     record field
    /// </summary>
    public class RecordField : SyntaxPartBase {

        /// <summary>
        ///     field type
        /// </summary>
        public TypeSpecification FieldType { get; set; }

        /// <summary>
        ///     hinting directive
        /// </summary>
        public HintingInformationList Hint { get; set; }


        /// <summary>
        ///     field names
        /// </summary>
        public IdentifierList Names { get; set; }

    }
}