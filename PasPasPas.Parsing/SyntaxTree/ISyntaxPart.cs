using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     basic interface for syntax tree elements
    /// </summary>
    public interface ISyntaxPart {

        /// <summary>
        ///     child nodes
        /// </summary>
        IEnumerable<ISyntaxPart> Parts { get; }

        /// <summary>
        ///     parent node
        /// </summary>
        ISyntaxPart Parent { get; set; }

        /// <summary>
        ///     accept a visitor object
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor);

    }


}