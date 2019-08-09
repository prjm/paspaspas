using PasPasPas.AssemblyBuilder.Builder;
using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Parsing;

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
        public AssemblyBuilderApi(IAssemblyBuilderEnvironment assemblyBuilderEnvironment, IOptionSet options) {
            SystemEnvironment = assemblyBuilderEnvironment;
            Options = options;
            Parser = new ParserApi(assemblyBuilderEnvironment, Options);
        }

        /// <summary>
        ///     create an assembly for a given project
        /// </summary>
        /// <param name="file"></param>
        /// <param name="resolver"></param>
        public IAssemblyReference CreateAssemblyForProject(IInputResolver resolver, FileReference file) {
            var data = Parser.Tokenizer.Readers.CreateInputForPath(file);
            using (var parser = Parser.CreateParser(resolver, file)) {
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
