using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     generic type ident
    /// </summary>
    public class GenericTypeIdent : SyntaxPartBase {

        /// <summary>
        ///     generic definition
        /// </summary>
        public GenericDefinition GenericDefinition { get; set; }

        /// <summary>
        ///     type name
        /// </summary>
        public PascalIdentifier Ident { get; set; }

    }
}