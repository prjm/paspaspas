using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Parsing.Parser;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Parser.Standard;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Common;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for the parser
    /// </summary>
    internal class ParserApi : IParserApi {

        /// <summary>
        ///     create a new parser API
        /// </summary>
        /// <param name="parserEnvironment"></param>
        /// <param name="options">options</param>
        public ParserApi(ITypedEnvironment parserEnvironment, IOptionSet options = null) {
            Environment = parserEnvironment;
            Options = options ?? new OptionSet(parserEnvironment);
            Tokenizer = new TokenizerApi(parserEnvironment, options);
        }

        /// <summary>
        ///     create a parser for a given input string
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        public IParser CreateParser(IReaderInput input) {
            var reader = Tokenizer.Readers.CreateReader(input);
            var parser = new StandardParser(Environment, Options, reader);
            return parser;
        }

        /// <summary>
        ///     create an abstract syntax tree
        /// </summary>
        /// <param name="bst">basic syntax tree</param>
        /// <returns>abstract syntax tree</returns>
        public ISyntaxPart CreateAbstractSyntraxTree(ISyntaxPart bst) {
            var root = new ProjectItemCollection();
            var visitor = new TreeTransformer(Environment, root);
            bst.Accept(visitor.AsVisitor());
            return root;
        }

        /*

        /// <summary>
        ///     process a complete project
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mainFile"></param>
        /// <returns></returns>
        public ProjectItemCollection CreateProjectForStrings(string path, string mainFile, string[] otherFiles) {
            var result = new ProjectItemCollection();
            var input = new Stack<string>();
            input.Push(path);
            while (input.Count > 0) {
                using (var parser = CreateParserForString(path, mainFile)) {
                    var cst = parser.Parse();
                    var visitor = new TreeTransformer(Environment, result);
                    cst.Accept(visitor.AsVisitor());
                }
            }

            return result;
        }
        */

        /// <summary>
        ///     used option sets
        /// </summary>
        public IOptionSet Options { get; }

        /// <summary>
        ///     annotate an abstract syntax tree with types
        /// </summary>
        /// <param name="ast">tree to annotate</param>
        public void AnnotateWithTypes(ISyntaxPart ast) {
            var typeVisitor = new TypeAnnotator(Environment);
            ast.Accept(typeVisitor.AsVisitor());
        }

        /// <summary>
        ///     access tokenizer functions
        /// </summary>
        public ITokenizerApi Tokenizer { get; }

        /// <summary>
        ///     system environment
        /// </summary>
        public ITypedEnvironment Environment { get; }

    }
}
