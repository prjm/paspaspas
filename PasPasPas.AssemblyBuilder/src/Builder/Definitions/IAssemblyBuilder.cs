using PasPasPas.Globals.Environment;

namespace PasPasPas.AssemblyBuilder.Builder.Definitions {

    /// <summary>
    ///     interface for an assembly builder
    /// </summary>
    public interface IAssemblyBuilder {

        /// <summary>
        ///     create an assembly reference
        /// </summary>
        /// <returns></returns>
        IAssemblyReference CreateAssemblyReference();

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
        ///     Start a unit definition
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns>unit type</returns>
        ITypeBuilder StartUnit(string symbolName);

    }
}
