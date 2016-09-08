namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     enumeration value
    /// </summary>
    public class EnumValue : SyntaxPartBase {

        /// <summary>
        ///     name
        /// </summary>
        public PascalIdentifier EnumName { get; set; }

        /// <summary>
        ///     value
        /// </summary>
        public Expression Value { get; set; }

    }
}