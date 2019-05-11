using System.Reflection;
using System.Reflection.Emit;
using PasPasPas.Globals.Environment;

namespace PasPasPas.AssemblyBuilder.Builder {

    /// <summary>
    ///     builder for .net assemblies
    /// </summary>
    internal class NetAssemblyBuilder : IAssemblyBuilder {

        private System.Reflection.Emit.AssemblyBuilder AsmBuilder { get; set; }
        private ModuleBuilder ModuleBuilder { get; set; }
        public Assembly GeneratedAssembly { get; private set; }

        public NetAssemblyBuilder() {

        }

        public void StartAssembly(string projectName) {
            var name = new AssemblyName(projectName);
            AsmBuilder = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = AsmBuilder.DefineDynamicModule("Demo");
        }

        public void EndAssembly() {
            if (ModuleBuilder != default)
                GeneratedAssembly = ModuleBuilder.Assembly;
        }

        public IAssemblyReference CreateAssemblyReference() {
            if (GeneratedAssembly == default)
                return default;
            return new AssemblyReference(GeneratedAssembly);
        }
    }
}