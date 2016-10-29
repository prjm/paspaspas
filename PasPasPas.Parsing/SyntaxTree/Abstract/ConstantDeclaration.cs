namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     declared constant
    /// </summary>
    public class ConstantDeclaration : DeclaredSymbol {

        /// <summary>
        ///     constant symbol name
        /// </summary>
        public override string SymbolName
            => Name.Name;

        /// <summary>
        ///     name of the constant
        /// </summary>
        public SymbolName Name { get; set; }

    }
}
