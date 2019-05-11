using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.AssemblyBuilder.Builder {

    /// <summary>
    ///     visitor to build an assembly
    /// </summary>
    public class ProjectAssemblyBuilder :

        IStartVisitor<ProjectItemCollection>, IEndVisitor<ProjectItemCollection> {

        private readonly IStartEndVisitor visitor;

        /// <summary>
        ///     environment
        /// </summary>
        public IAssemblyBuilderEnvironment Environment { get; }

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource LogSource { get; }

        /// <summary>
        ///     module builder
        /// </summary>
        private IAssemblyBuilder Builder { get; }

        /// <summary>
        ///     create a new builder
        /// </summary>
        /// <param name="environment"></param>
        public ProjectAssemblyBuilder(IAssemblyBuilderEnvironment environment) {
            visitor = new ChildVisitor(this);
            Environment = environment;
            LogSource = environment.Log.CreateLogSource(MessageGroups.AssemblyBuilder);
            Builder = new NetAssemblyBuilder();
        }

        /// <summary>
        ///     helper object
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor()
            => visitor;

        /// <summary>
        ///     start visiting a project
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ProjectItemCollection element) {
            var projectName = element.ProjectName;

            if (string.IsNullOrWhiteSpace(projectName))
                LogError(BuilderErrorMessages.UndefinedProjectName);
            else
                Builder.StartAssembly(projectName);
        }

        private void LogError(uint messageNumber)
            => LogSource.LogError(messageNumber);

        /// <summary>
        ///     end visiting a project
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ProjectItemCollection element)
            => Builder.EndAssembly();

        /// <summary>
        ///     create a new assembly reference
        /// </summary>
        /// <returns></returns>
        public IAssemblyReference CreateAssemblyReference()
            => Builder.CreateAssemblyReference();
    }
}
