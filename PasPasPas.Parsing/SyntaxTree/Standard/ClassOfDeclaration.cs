namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class of declaration
    /// </summary>
    public class ClassOfDeclaration : SyntaxPartBase {

        /// <summary>
        ///     type name
        /// </summary>
        public TypeName TypeRef { get; set; }

    }
}