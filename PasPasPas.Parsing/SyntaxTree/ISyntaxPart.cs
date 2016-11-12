using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     interface for syntax tree elements
    /// </summary>
    public interface ISyntaxPart {

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to accept</param>
        /// <param name="visitorParameter">parameter</param>
        bool Accept<T>(ISyntaxPartVisitor<T> visitor, T visitorParameter);

        /// <summary>
        ///     child nodes
        /// </summary>
        IEnumerable<ISyntaxPart> Parts { get; }

        /// <summary>
        ///     parent node
        /// </summary>
        ISyntaxPart Parent { get; set; }

    }


    /// <summary>
    ///     extendable syntax part
    /// </summary>
    public interface IExtendableSyntaxPart : ISyntaxPart {

        /// <summary>
        ///     add an part
        /// </summary>
        /// <param name="result"></param>
        void Add(ISyntaxPart result);

        /// <summary>
        ///     remove an part
        /// </summary>
        /// <param name="lastSymbol"></param>
        void Remove(ISyntaxPart lastSymbol);
    }

}