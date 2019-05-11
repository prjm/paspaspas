using PasPasPas.Globals.Environment;

namespace PasPasPas.AssemblyBuilder.Builder {

    /// <summary>
    ///     interface for an assembly builder
    /// </summary>
    public interface IAssemblyBuilder {

        /// <summary>
        ///     start building an assembly
        /// </summary>
        /// <param name="projectName">project name</param>
        void StartAssembly(string projectName);

        /// <summary>
        ///     end building an assembly
        /// </summary>
        void EndAssembly();

        /// <summary>
        ///     create an assembly reference
        /// </summary>
        /// <returns></returns>
        IAssemblyReference CreateAssemblyReference();
    }
}
