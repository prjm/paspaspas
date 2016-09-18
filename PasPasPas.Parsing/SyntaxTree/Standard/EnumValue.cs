namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     enumeration value
    /// </summary>
    public class EnumValue : SyntaxPartBase {

        /// <summary>
        ///     name
        /// </summary>
        public Identifier EnumName { get; set; }

        /// <summary>
        ///     value
        /// </summary>
        public Expression Value { get; set; }

    }
}