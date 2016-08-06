using PasPasPas.Api;
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
        /// <param name="param">parameter</param>
        void Accept<TParam>(ISyntaxPartVisitor<TParam> visitor, TParam param);

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