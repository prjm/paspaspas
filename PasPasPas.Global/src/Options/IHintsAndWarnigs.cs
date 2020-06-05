#nullable disable
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     options for hints and warnings
    /// </summary>
    public interface IHintsAndWarnigs {

        /// <summary>
        ///     hint options
        /// </summary>
        IOption<CompilerHint> Hints { get; }

        /// <summary>
        ///     warning mode
        /// </summary>
        IOption<CompilerWarning> Warnings { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();

        /// <summary>
        ///     reset options on a new unit
        /// </summary>
        void ResetOnNewUnit();
    }
}