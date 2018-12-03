namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     options for hints and warnings
    /// </summary>
    public class HintsAndWarningsOptions {

        /// <summary>
        ///     create new settings for hints or warnings
        /// </summary>
        /// <param name="baseOptions"></param>
        public HintsAndWarningsOptions(HintsAndWarningsOptions baseOptions) {
            Hints = new DerivedValueOption<CompilerHint>(baseOptions?.Hints);
            Warnings = new DerivedValueOption<CompilerWarning>(baseOptions?.Warnings);
        }

        /// <summary>
        ///     enable or disable hints
        /// </summary>
        public DerivedValueOption<CompilerHint> Hints { get; }

        /// <summary>
        ///     compiler warnings
        /// </summary>
        public DerivedValueOption<CompilerWarning> Warnings { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        public void Clear() {
            Hints.ResetToDefault();
            Warnings.ResetToDefault();
        }

        /// <summary>
        ///     reset on a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            Hints.ResetToDefault();
            Warnings.ResetToDefault();
        }
    }
}
