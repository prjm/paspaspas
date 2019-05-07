using System.Reflection;
using System.Reflection.Emit;
using Lokad.ILPack;
using PasPasPas.Globals.Environment;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.AssemblyBuilder.Builder {

    /// <summary>
    ///     visitor to build an assembly
    /// </summary>
    public class ProjectAssemblyBuilder :


        IStartVisitor<ProjectItemCollection>,
        IEndVisitor<ProjectItemCollection> {

        private readonly IStartEndVisitor visitor;

        /// <summary>
        ///     environment
        /// </summary>
        public IAssemblyBuilderEnvironment Environment { get; }

        /// <summary>
        ///     assembly builder
        /// </summary>
        public System.Reflection.Emit.AssemblyBuilder Builder { get; }

        /// <summary>
        ///     module builder
        /// </summary>
        private ModuleBuilder Module { get; }

        /// <summary>
        ///     create a new builder
        /// </summary>
        /// <param name="environment"></param>
        public ProjectAssemblyBuilder(IAssemblyBuilderEnvironment environment) {
            visitor = new ChildVisitor(this);
            Environment = environment;
            var name = new AssemblyName("Demo");
            Builder = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndCollect);
            Module = Builder.DefineDynamicModule("Demo");
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
        }

        /// <summary>
        ///     end visiting a project
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ProjectItemCollection element) {
        }

        /// <summary>
        ///     save the assembly to a file
        /// </summary>
        public void SaveToFile() {
            var assembly = Module.Assembly;
            var generator = new AssemblyGenerator();
            generator.GenerateAssembly(assembly, @"C:\temp\Demo.dll");
        }
    }
}
