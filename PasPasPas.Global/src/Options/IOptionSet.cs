using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     interface for a set of option
    /// </summary>
    public interface IOptionSet {

        /// <summary>
        ///     compiler options
        /// </summary>
        ICompilerOptions CompilerOptions { get; }

        /// <summary>
        ///     warning options
        /// </summary>
        IWarningOptions Warnings { get; }

        /// <summary>
        ///     conditional compilation options
        /// </summary>
        IConditionalCompilationOptions ConditionalCompilation { get; }

        /// <summary>
        ///     meta options
        /// </summary>
        IMetaOptions Meta { get; }

        /// <summary>
        ///     path options
        /// </summary>
        IPathOptions PathOptions { get; }

        /// <summary>
        ///     environment
        /// </summary>
        IEnvironment Environment { get; }

        /// <summary>
        ///     switch info result
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        SwitchInfo GetSwitchInfo(string value);

        /// <summary>
        ///     clear all options
        /// </summary>
        void Clear();

        /// <summary>
        ///     reset options on a new unit
        /// </summary>
        void ResetOnNewUnit();
    }
}
