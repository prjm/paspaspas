namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     additional compiler options
    /// </summary>
    public class AdditionalCompilerOptions {
        public AdditionalCompilerOptions(AdditionalCompilerOptions baseOptions) {
            LegacyIfEnd = new DerivedValueOption<EndIfMode>(baseOptions?.LegacyIfEnd);
            ExtendedCompatibility = new DerivedValueOption<ExtendedCompatibilityMode>(baseOptions?.ExtendedCompatibility);
            ExcessPrecision = new DerivedValueOption<ExcessPrecisionForResult>(baseOptions?.ExcessPrecision);
            HighCharUnicode = new DerivedValueOption<HighCharsUnicode>(baseOptions?.HighCharUnicode);
            ImplicitBuild = new DerivedValueOption<ImplicitBuildUnit>(baseOptions?.ImplicitBuild);
            RealCompatibility = new DerivedValueOption<Real48>(baseOptions?.RealCompatibility);
            OldTypeLayout = new DerivedValueOption<OldRecordTypeMode>(baseOptions?.OldTypeLayout);
        }

        /// <summary>
        ///     legacy if / end if mode
        /// </summary>
        public DerivedValueOption<EndIfMode> LegacyIfEnd { get; }

        /// <summary>
        ///     extended compatibility mode
        /// </summary>
        public DerivedValueOption<ExtendedCompatibilityMode> ExtendedCompatibility { get; }

        /// <summary>
        ///     excess precision on x64
        /// </summary>
        public DerivedValueOption<ExcessPrecisionForResult> ExcessPrecision { get; }

        /// <summary>
        ///     high chars for unicode
        /// </summary>
        public DerivedValueOption<HighCharsUnicode> HighCharUnicode { get; }

        /// <summary>
        ///     implicit build setting
        /// </summary>
        public DerivedValueOption<ImplicitBuildUnit> ImplicitBuild { get; }

        /// <summary>
        ///     switch for 48-bit doubles
        /// </summary>
        public DerivedValueOption<Real48> RealCompatibility { get; }

        /// <summary>
        ///     switch for old record types
        /// </summary>
        public DerivedValueOption<OldRecordTypeMode> OldTypeLayout { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        public void Clear() {
            LegacyIfEnd.ResetToDefault();
            ExtendedCompatibility.ResetToDefault();
            ExcessPrecision.ResetToDefault();
            HighCharUnicode.ResetToDefault();
            ImplicitBuild.ResetToDefault();
            RealCompatibility.ResetToDefault();
            OldTypeLayout.ResetToDefault();
        }

        /// <summary>
        ///     reset on a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            LegacyIfEnd.ResetToDefault();
            ExtendedCompatibility.ResetToDefault();
            ExcessPrecision.ResetToDefault();
            HighCharUnicode.ResetToDefault();
            RealCompatibility.ResetToDefault();
            OldTypeLayout.ResetToDefault();
        }
    }
}
