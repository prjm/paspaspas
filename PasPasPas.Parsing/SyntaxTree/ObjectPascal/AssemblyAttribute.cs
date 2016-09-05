namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     assembly attribute
    /// </summary>
    public class AssemblyAttribute : SyntaxPartBase {

        /// <summary>
        ///     attribute definition
        /// </summary>
        public UserAttribute Attribute { get; set; }

    }
}
