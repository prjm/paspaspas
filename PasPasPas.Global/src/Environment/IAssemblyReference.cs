using System.Reflection;

namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     reference to a generated assembly
    /// </summary>
    public interface IAssemblyReference {

        /// <summary>
        ///     generated assembly
        /// </summary>
        Assembly GeneratedAssembly { get; }

    }
}
