using PasPasPas.Options.DataTypes;

namespace PasPasPas.Options.Bundles {

    /// <summary>
    ///     interface for option sets
    /// </summary>
    public interface IOptionSet {

        /// <summary>
        ///     compiler options
        /// </summary>
        CompileOptions CompilerOptions { get; }

        /// <summary>
        ///     conditional compilation options
        /// </summary>
        ConditionalCompilationOptions ConditionalCompilation { get; }

        /// <summary>
        ///     meta information
        /// </summary>
        MetaInformation Meta { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();

        /// <summary>
        ///     clear intermediate options
        /// </summary>
        void ResetOnNewUnit();
    }
}