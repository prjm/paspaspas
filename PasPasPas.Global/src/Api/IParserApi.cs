using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Parsing.Parser;

namespace PasPasPas.Globals.Api {

    /// <summary>
    ///     parser service API
    /// </summary>
    public interface IParserApi {

        /// <summary>
        ///     tokenizer API
        /// </summary>
        ITokenizerApi Tokenizer { get; }

        /// <summary>
        ///     options
        /// </summary>
        IOptionSet Options { get; }

        /// <summary>
        ///     create a new parser
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IParser CreateParser(IFileReference path);

        /// <summary>
        ///     create an abstract syntax tree
        /// </summary>
        /// <param name="concreteSyntaxTree"></param>
        /// <returns></returns>
        ISyntaxPart CreateAbstractSyntraxTree(ISyntaxPart concreteSyntaxTree);

        /// <summary>
        ///     add type attributes to the abstract syntax tree
        /// </summary>
        /// <param name="syntaxTree"></param>
        void AnnotateWithTypes(ISyntaxPart syntaxTree);
    }
}
