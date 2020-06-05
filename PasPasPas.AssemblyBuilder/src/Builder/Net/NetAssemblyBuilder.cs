#nullable disable
using System.Reflection;
using System.Reflection.Emit;
using PasPasPas.AssemblyBuilder.Builder.Definitions;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Types;

namespace PasPasPas.AssemblyBuilder.Builder.Net {

    /// <summary>
    ///     builder for .net assemblies
    /// </summary>
    internal class NetAssemblyBuilder : IAssemblyBuilder {

        private const string AssemblyNamespacePrefix = "P3.";

        private System.Reflection.Emit.AssemblyBuilder AsmBuilder { get; set; }
        private ModuleBuilder ModuleBuilder { get; set; }
        private string UnitNamespace { get; set; }
        private TypeBuilder UnitClass { get; set; }
        private TypeMapper Mapper { get; }
        public Assembly GeneratedAssembly { get; private set; }

        public NetAssemblyBuilder(ITypeRegistry types)
            => Mapper = new TypeMapper(types);

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

        public ITypeBuilder StartUnit(string unitName) {
            UnitNamespace = AssemblyNamespacePrefix + unitName.Replace('.', '_');
            UnitClass = ModuleBuilder.DefineType(UnitNamespace + ".<UnitClass>");
            return new NetTypeBuilder(UnitClass, Mapper);
        }

    }
}