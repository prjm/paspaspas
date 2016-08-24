namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     no define directive
    /// </summary>
    public class NoDefine : SyntaxPartBase {

        /// <summary>
        ///     type name
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        ///     tpye names in header files
        /// </summary>
        public string TypeNameInHpp { get; set; }

        /// <summary>
        ///     type name in unions
        /// </summary>
        public string TypeNameInUnion { get; set; }
    }
}
