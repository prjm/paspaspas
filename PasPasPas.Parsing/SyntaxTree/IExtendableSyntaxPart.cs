using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree {
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
