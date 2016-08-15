using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
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
        ///     warning options
        /// </summary>
        WarningOptions Warnings { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();

        /// <summary>
        ///     clear intermediate options
        /// </summary>
        /// <param name="logManager">log manager for error messages</param>
        void ResetOnNewUnit(ILogManager logManager);

        /// <summary>
        ///     get information for a switch
        /// </summary>
        /// <param name="switchKind">switch kind</param>
        /// <returns>switch infi</returns>
        SwitchInfo GetSwitchInfo(string switchKind);

        /// <summary>
        ///     file access
        /// </summary>
        IFileAccess Files { get; }

    }
}