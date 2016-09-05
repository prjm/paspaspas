using PasPasPas.Api;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     interface for syntax tree elements
    /// </summary>
    public interface ISyntaxTreeNode {

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to accept</param>
        /// <param name="visitorParameter">parameter</param>
        bool Accept<T>(ISyntaxTreeNodeVisitor<T> visitor, T visitorParameter);

        /// <summary>
        ///     child nodes
        /// </summary>
        IEnumerable<ISyntaxTreeNode> Parts { get; }

        /// <summary>
        ///     parent node
        /// </summary>
        ISyntaxTreeNode Parent { get; set; }

    }

    /// <summary>
    ///     temporary interface
    /// </summary>
    public interface IFormattableSyntaxPart : ISyntaxTreeNode {


        /// <summary>
        ///     print a pascal representation of this program part
        /// </summary>        
        /// <param name="result">output</param>
        void ToFormatter(PascalFormatter result);

    }

}