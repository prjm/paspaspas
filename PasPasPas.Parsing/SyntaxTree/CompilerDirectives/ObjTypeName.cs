namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     object type name directive
    /// </summary>
    public class ObjTypeName : SyntaxPartBase {

        /// <summary>
        ///     alias name
        /// </summary>
        public string AliasName { get; internal set; }

        /// <summary>
        ///     type name in object file
        /// </summary>
        public string TypeName { get; internal set; }
    }
}
