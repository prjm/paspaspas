namespace PasPasPas.Api.Options {

    /// <summary>
    ///     set of compiler options
    /// </summary>
    public class OptionSet {

        /// <summary>
        ///     debug configuration
        /// </summary>
        public const string DebugConfigurationName = "Debug";

        /// <summary>
        ///     release configuration
        /// </summary>
        public const string ReleaseConfigurationName = "Release";

        /// <summary>
        ///     creates a new option set
        /// </summary>
        public OptionSet() : this(null) { }

        /// <summary>
        ///     create a new option set
        /// </summary>
        /// <param name="baseOptions"></param>
        public OptionSet(OptionSet baseOptions) {
            CompilerOptions = new CompileOptions(baseOptions?.CompilerOptions);
        }

        /// <summary>
        ///     compiler-related optiosn
        /// </summary>
        public CompileOptions CompilerOptions { get; }


        /// <summary>
        ///     clear all option values
        /// </summary>
        public void Clear() {
            CompilerOptions.Clear();
        }
    }
}