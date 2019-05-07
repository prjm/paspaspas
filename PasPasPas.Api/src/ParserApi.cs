using PasPasPas.Globals.Environment;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.Parser.Standard;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Common;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for the parser
    /// </summary>
    public class ParserApi {

        /// <summary>
        ///     create a new parser API
        /// </summary>
        /// <param name="parserEnvironment"></param>
        /// <param name="options">options</param>
        public ParserApi(ITypedEnvironment parserEnvironment, OptionSet options = null) {
            Environment = parserEnvironment;
            Options = options ?? new OptionSet(parserEnvironment);
            Tokenizer = new TokenizerApi(parserEnvironment, options);
        }

        /// <summary>
        ///     create a parser for a given input string
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="input">input</param>
        /// <returns></returns>
        public IParser CreateParserForString(string path, string input) {
            var reader = ReaderApi.CreateReaderForString(path, input);
            var parser = new StandardParser(Environment, Options, reader);
            return parser;
        }

        /// <summary>
        ///     create a parser for a given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IParser CreateParserForPath(string path) {
            var reader = ReaderApi.CreateReaderForPath(path);
            var parser = new StandardParser(Environment, Options, reader);
            return parser;
        }

        /// <summary>
        ///     create an abstract syntax tree
        /// </summary>
        /// <param name="bst">basic syntax tree</param>
        /// <returns>abstract syntax tree</returns>
        public ProjectItemCollection CreateAbstractSyntraxTree(ISyntaxPart bst) {
            var root = new ProjectItemCollection();
            var visitor = new TreeTransformer(Environment, root);
            bst.Accept(visitor.AsVisitor());
            return root;
        }

        /// <summary>
        ///     used option sets
        /// </summary>
        public OptionSet Options { get; }

        /// <summary>
        ///     annotate an abstract syntax tree with types
        /// </summary>
        /// <param name="ast">tree to annotate</param>
        public void AnnotateWithTypes(ProjectItemCollection ast) {
            var typeVisitor = new TypeAnnotator(Environment);
            ast.Accept(typeVisitor.AsVisitor());
        }

        /// <summary>
        ///     access tokenizer functions
        /// </summary>
        public TokenizerApi Tokenizer { get; }

        /// <summary>
        ///     system environment
        /// </summary>
        public ITypedEnvironment Environment { get; }

    }
}
