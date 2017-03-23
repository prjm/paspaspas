using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     directive target
    /// </summary>
    public interface IDirectiveTarget {

        /// <summary>
        ///     directives
        /// </summary>
        IList<MethodDirective> Directives { get; }

        /// <summary>
        ///     hints
        /// </summary>
        SymbolHints Hints { get; set; }
    }
}