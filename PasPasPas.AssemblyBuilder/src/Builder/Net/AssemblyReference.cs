#nullable disable
using System.Reflection;
using PasPasPas.Globals.Environment;

namespace PasPasPas.AssemblyBuilder.Builder.Net {

    /// <summary>
    ///     assembly reference
    /// </summary>
    internal class AssemblyReference : IAssemblyReference {

        /// <summary>
        ///     create a new assembly reference
        /// </summary>
        /// <param name="generatedAssembly"></param>
        public AssemblyReference(Assembly generatedAssembly)
            => GeneratedAssembly = generatedAssembly;

        /// <summary>
        ///     assembly reference
        /// </summary>
        public Assembly GeneratedAssembly { get; }

    }
}