using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     base class for declaration items
    /// </summary>
    public abstract class DeclarationBase : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        protected DeclarationBase(IParserInformationProvider informationProvider) : base(informationProvider) { }

    }
}