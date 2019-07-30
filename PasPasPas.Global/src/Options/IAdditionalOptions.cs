using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     additional options
    /// </summary>
    public interface IAdditionalOptions {

        /// <summary>
        ///     extended compatibility mode
        /// </summary>
        IOption<ExtendedCompatibilityMode> ExtendedCompatibility { get; }

        /// <summary>
        ///     excess precision
        /// </summary>
        IOption<ExcessPrecisionForResult> ExcessPrecision { get; }

        /// <summary>
        ///     option for high char unicode
        /// </summary>
        IOption<HighCharsUnicode> HighCharUnicode { get; }

        /// <summary>
        ///     implicit build option
        /// </summary>
        IOption<ImplicitBuildUnit> ImplicitBuild { get; }

        /// <summary>
        ///     real compatibility
        /// </summary>
        IOption<Real48> RealCompatibility { get; }

        /// <summary>
        ///     old record type mode
        /// </summary>
        IOption<OldRecordTypeMode> OldTypeLayout { get; }

        /// <summary>
        ///     legacy if end
        /// </summary>
        IOption<EndIfMode> LegacyIfEnd { get; }

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