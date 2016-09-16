namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     record declaration
    /// </summary>
    public class RecordDeclaration : SyntaxPartBase {

        /// <summary>
        ///     field list
        /// </summary>
        public RecordFieldList FieldList { get; set; }

        /// <summary>
        ///     record items
        /// </summary>
        public RecordItems Items { get; set; }

        /// <summary>
        ///     variant section
        /// </summary>
        public RecordVariantSection VariantSection { get; set; }

    }
}