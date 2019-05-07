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
        public void CreateAssemblyForProject(string path) {
            using (var parser = Parser.CreateParserForPath(path)) {
                var result = parser.Parse();
                var project = Parser.CreateAbstractSyntraxTree(result);
                Parser.AnnotateWithTypes(project);
                CreateAssembly(project);
            }
        }

        /// <summary>
        ///     create a assembly for a given project
        /// </summary>
        /// <param name="project"></param>
        private void CreateAssembly(ProjectItemCollection project) {
            var builder = new ProjectAssemblyBuilder(SystemEnvironment);
            project.Accept(builder.AsVisitor());
            builder.SaveToFile();
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
