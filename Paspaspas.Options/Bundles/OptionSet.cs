using PasPasPas.Options.DataTypes;

namespace PasPasPas.Options.Bundles {

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
            ConditionalCompilation = new ConditionalCompilationOptions(baseOptions?.ConditionalCompilation);
            Meta = new MetaInformation(baseOptions?.Meta);
        }

        /// <summary>
        ///     compiler-related options
        /// </summary>
        public CompileOptions CompilerOptions { get; }

        /// <summary>
        ///     conditional compilation options
        /// </summary>
        public ConditionalCompilationOptions ConditionalCompilation { get; }

        /// <summary>
        ///     meta information
        /// </summary>
        public MetaInformation Meta { get; }

        /// <summary>
        ///     clear all option values
        /// </summary>
        public void Clear() {
            CompilerOptions.Clear();
            ConditionalCompilation.Clear();
            Meta.Clear();
        }

        /// <summary>
        ///     reset definitions for a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            CompilerOptions.ResetOnNewUnit();
            ConditionalCompilation.ResetOnNewUnit();
            Meta.ResetOnNewUnit();
        }
    }
}