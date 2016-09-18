namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     struct type
    /// </summary>
    public class StructType : SyntaxPartBase {

        /// <summary>
        ///     Packed struct type
        /// </summary>
        public bool Packed { get; set; }

        /// <summary>
        ///     part
        /// </summary>
        public StructTypePart Part { get; set; }

    }
}