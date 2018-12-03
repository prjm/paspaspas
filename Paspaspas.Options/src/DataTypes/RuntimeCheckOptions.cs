namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     runtime check options
    /// </summary>
    public class RuntimeCheckOptions {

        /// <summary>
        ///     create new runtime check options
        /// </summary>
        /// <param name="baseOptions"></param>
        public RuntimeCheckOptions(RuntimeCheckOptions baseOptions) {
            RangeChecks = new DerivedValueOption<RuntimeRangeCheckMode>(baseOptions?.RangeChecks);
            IoChecks = new DerivedValueOption<IoCallCheck>(baseOptions?.IoChecks);
            CheckOverflows = new DerivedValueOption<RuntimeOverflowCheck>(baseOptions?.CheckOverflows);
        }

        /// <summary>
        ///     generate runtime range checks
        /// </summary>
        public DerivedValueOption<RuntimeRangeCheckMode> RangeChecks { get; }

        /// <summary>
        ///     io checks flag
        /// </summary>
        public DerivedValueOption<IoCallCheck> IoChecks { get; }

        /// <summary>
        ///     flag to enable overflow checks
        /// </summary>
        public DerivedValueOption<RuntimeOverflowCheck> CheckOverflows { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        public void Clear() {
            RangeChecks.ResetToDefault();
            IoChecks.ResetToDefault();
            CheckOverflows.ResetToDefault();
        }

        /// <summary>
        ///     reset flags for a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            RangeChecks.ResetToDefault();
            IoChecks.ResetToDefault();
            CheckOverflows.ResetToDefault();
        }
    }
}
