using PasPasPas.AssemblyBuilder.Builder;
using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;
using PasPasPas.Parsing.SyntaxTree.Abstract;

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
            Parser = new ParserApi(Options);
        }

        /// <summary>
        ///     create an assembly for a given project
        /// </summary>
        /// <param name="file"></param>
        public IAssemblyReference CreateAssemblyForProject(IFileReference file) {
            using (var parser = Parser.CreateParser(file)) {
                var result = parser.Parse();
                var project = Parser.CreateAbstractSyntraxTree(result) as ProjectItemCollection;
                Parser.AnnotateWithTypes(project);
                return CreateAssembly(project.ProjectName);
            }
        }

        /// <summary>
        ///     create a assembly for a given project
        /// </summary>
        /// <param name="projectName"></param>
        private IAssemblyReference CreateAssembly(string projectName) {
            var builder = new ProjectAssemblyBuilder(SystemEnvironment);
            builder.PrepareAssembly(projectName);
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
