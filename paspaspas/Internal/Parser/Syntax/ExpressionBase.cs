using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     base class for expressions
    /// </summary>
    public abstract class ExpressionBase : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        protected ExpressionBase(IParserInformationProvider informationProvider) : base(informationProvider) { }

    }
}