using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Utils {

    /// <summary>
    ///     common interface for syntax tree elements
    /// </summary>
    public interface ISyntaxPart {

        /// <summary>
        ///     child nodes
        /// </summary>
        IEnumerable<ISyntaxPart> Parts { get; }

        /// <summary>
        ///     parent syntax tree node
        /// </summary>
        ISyntaxPart ParentItem { get; set; }

        /// <summary>
        ///     accept a visitor object
        /// </summary>
        /// <param name="visitor">visitor</param>
        void Accept(IStartEndVisitor visitor);

    }


}