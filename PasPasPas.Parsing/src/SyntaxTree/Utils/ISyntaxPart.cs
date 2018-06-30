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
        ///     length (number of characters) of a syntax item
        /// </summary>
        int Length { get; }

        /// <summary>
        ///     accept a visitor object
        /// </summary>
        /// <param name="visitor">visitor</param>
        void Accept(IStartEndVisitor visitor);

    }


    /// <summary>
    ///     helper for syntax parts
    /// </summary>
    public static class SyntaxPartHelper {

        /// <summary>
        ///     get the symbol length
        /// </summary>
        /// <param name="part">part length</param>
        /// <returns>length or <c>0</c></returns>
        public static int GetSymbolLength(this ISyntaxPart part)
            => part == null ? 0 : part.Length;

    }

}