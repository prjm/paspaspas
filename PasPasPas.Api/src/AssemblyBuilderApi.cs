using PasPasPas.AssemblyBuilder.Builder;
using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Parsing;
using PasPasPas.Options.Bundles;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for assembly building
    /// </summary>
    public class AssemblyBuilderApi {

        /// <summary>
        ///     create a new assembly builder API
        /// </summary>
        /// <param name="assemblyBuilderEnvironment"></param>
        /// <param name="options">options</param>
        public AssemblyBuilderApi(IAssemblyBuilderEnvironment assemblyBuilderEnvironment, OptionSet options = null) {
            SystemEnvironment = assemblyBuilderEnvironment;
            Options = options ?? new OptionSet(assemblyBuilderEnvironment);
            Parser = new ParserApi(assemblyBuilderEnvironment, Options);
        }

        /// <summary>
        ///     create an assembly for a given project
        /// </summary>
        /// <param name="path"></param>
        public IAssemblyReference CreateAssemblyForProject(string path) {
            var data = Parser.Tokenizer.Readers.CreateInputForPath(path);
            using (var parser = Parser.CreateParser(data)) {
                var result = parser.Parse();
                var project = Parser.CreateAbstractSyntraxTree(result);
                Parser.AnnotateWithTypes(project);
                return CreateAssembly(project);
            }
        }

        /// <summary>
        ///     create an assembly for a given input string
        /// </summary>
        /// <param name="input">input</param>
        public IAssemblyReference CreateAssembly(IReaderInput input) {
            using (var parser = Parser.CreateParser(input)) {
                var result = parser.Parse();
                var project = Parser.CreateAbstractSyntraxTree(result);
                Parser.AnnotateWithTypes(project);
                return CreateAssembly(project);
            }
        }

        /// <summary>
        ///     create a assembly for a given project
        /// </summary>
        /// <param name="project"></param>
        private IAssemblyReference CreateAssembly(ISyntaxPart project) {
            var builder = new ProjectAssemblyBuilder(SystemEnvironment);
            project.Accept(builder.AsVisitor());
            return builder.CreateAssemblyReference();
        }

        /// <summary>
        ///     common environment
        /// </summary>
        public IAssemblyBuilderEnvironment SystemEnvironment { get; }

        /// <summary>
        ///     options
        /// </summary>
        public IOptionSet Options { get; }

        /// <summary>
        ///     parser API
        /// </summary>
        public IParserApi Parser { get; }
    }
}
