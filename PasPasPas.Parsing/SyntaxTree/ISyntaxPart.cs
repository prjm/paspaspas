using PasPasPas.Api;
using PasPasPas.Parsing.Parser;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     interface for syntax tree elements
    /// </summary>
    public interface ISyntaxPart {

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor"></param>
        void Accept(ISyntaxPartVisitor visitor);

        /// <summary>
        /// 
        /// </summary>
        ICollection<ISyntaxPart> Parts { get; }

    }

    /// <summary>
    ///     temporary interface
    /// </summary>
    public interface IFormattableSyntaxPart : ISyntaxPart {


        /// <summary>
        ///     print a pascal representation of this program part
        /// </summary>        
        /// <param name="result">output</param>
        void ToFormatter(PascalFormatter result);

    }

}