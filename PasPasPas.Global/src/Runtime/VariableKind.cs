#nullable disable
namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     variable kind
    /// </summary>
    public enum VariableKind {

        /// <summary>
        ///     unknown variable
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     global variable
        /// </summary>
        GlobalVariable = 1,

        /// <summary>
        ///     local variable
        /// </summary>
        LocalVariable = 2,

    }
}