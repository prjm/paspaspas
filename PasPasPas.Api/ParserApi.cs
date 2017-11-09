using System;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Common;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for tokenizer api
    /// </summary>
    public class ParserApi {

        private readonly ITypedEnvironment env;
        private readonly TokenizerApi tokenizerApi;
        private readonly OptionSet parseOptions;

        /// <summary>
        ///     create a new parser api
        /// </summary>
        /// <param name="parserEnvironment"></param>
        /// <param name="options">options</param>
        public ParserApi(ITypedEnvironment parserEnvironment, OptionSet options = null) {
            env = parserEnvironment;
            parseOptions = options ?? new OptionSet(parserEnvironment);
            tokenizerApi = new TokenizerApi(parserEnvironment, options);
        }

        /// <summary>
        ///     create a parser for a given input string
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="input">input</param>
        /// <returns></returns>
        public IParser CreateParserForString(string path, string input) {
            var reader = tokenizerApi.Readers.CreateReaderForString(path, input);
            var parser = new StandardParser(env, parseOptions, reader);
            return parser;
        }
        /// <summary>
        ///     create a parser for a given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IParser CreateParserForPath(string path) {
            var reader = tokenizerApi.Readers.CreateReaderForPath(path);
            var parser = new StandardParser(env, parseOptions, reader);
            return parser;
        }

        /// <summary>
        ///     create an abstract syntax tree
        /// </summary>
        /// <param name="bst">basic syntax tree</param>
        /// <returns>abstract syntax tree</returns>
        public ProjectRoot CreateAbstractSyntraxTree(ISyntaxPart bst) {
            var root = new ProjectRoot();
            var visitor = new TreeTransformer(env, root);
            bst.Accept(visitor.AsVisitor());
            return root;
        }

        /// <summary>
        ///     used option sets
        /// </summary>
        public OptionSet Options
            => parseOptions;

        /// <summary>
        ///     annotate an abstract syntax tree with types
        /// </summary>
        /// <param name="ast">tree to annotate</param>
        public void AnnotateWithTypes(ProjectRoot ast) {
            var typeVisitor = new TypeAnnotator(env);
            ast.Accept(typeVisitor.AsVisitor());
        }
    }
}
