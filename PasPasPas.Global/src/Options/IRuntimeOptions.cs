#nullable disable
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     runtime options
    /// </summary>
    public interface IRuntimeOptions {

        /// <summary>
        ///     enable io call checks
        /// </summary>
        IOption<IoCallCheck> IoChecks { get; }

        /// <summary>
        ///     check overflows
        /// </summary>
        IOption<RuntimeOverflowCheck> CheckOverflows { get; }

        /// <summary>
        ///     range checks
        /// </summary>
        IOption<RuntimeRangeCheckMode> RangeChecks { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();

        /// <summary>
        ///     clear options on a new unit
        /// </summary>
        void ResetOnNewUnit();

    }
}