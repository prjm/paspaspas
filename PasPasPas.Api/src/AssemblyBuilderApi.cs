using PasPasPas.AssemblyBuilder.Builder;
using PasPasPas.Globals.Environment;
using PasPasPas.Options.Bundles;
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
            using (var parser = Parser.CreateParserForPath(path)) {
                var result = parser.Parse();
                var project = Parser.CreateAbstractSyntraxTree(result);
                Parser.AnnotateWithTypes(project);
                return CreateAssembly(project);
            }
        }

        /// <summary>
        ///     create an assembly for a given input string
        /// </summary>
        /// <param name="program"></param>
        /// <param name="file"></param>
        public IAssemblyReference CreateAssemblyForString(string file, string program) {
            using (var parser = Parser.CreateParserForString(file, program)) {
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
        private IAssemblyReference CreateAssembly(ProjectItemCollection project) {
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
        public OptionSet Options { get; }

        /// <summary>
        ///     parser API
        /// </summary>
        public ParserApi Parser { get; }
    }
}
