using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     directive target
    /// </summary>
    public interface IDirectiveTarget {

        /// <summary>
        ///     directives
        /// </summary>
        ISyntaxPartList<MethodDirective> Directives { get; }

        /// <summary>
        ///     hints
        /// </summary>
        SymbolHints Hints { get; set; }
    }
}