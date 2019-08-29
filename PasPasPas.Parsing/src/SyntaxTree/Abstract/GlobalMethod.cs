namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a global method declaration
    /// </summary>
    public class GlobalMethod : MethodDeclaration, IMethodImplementation {

        /// <summary>
        ///     anchor
        /// </summary>
        public SingleDeclaredSymbol Anchor { get; set; }

        /// <summary>
        ///     forward declaration
        /// </summary>
        public bool IsForwardDeclaration
            => false;

        /// <summary>
        ///     exported method
        /// </summary>
        public bool IsExportedMethod
            => false;

        /// <summary>
        ///     global method
        /// </summary>
        public bool IsGlobalMethod
            => true;
    }
}
