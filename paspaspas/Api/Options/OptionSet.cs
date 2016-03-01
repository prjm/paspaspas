namespace PasPasPas.Api.Options {

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
            Align = new DerivedValueOption<Alignment>(baseOptions?.Align);
            ApplicationType = new DerivedValueOption<AppType>(baseOptions?.ApplicationType);
            BoolEval = new DerivedValueOption<BooleanEvaluation>(baseOptions?.BoolEval);
        }

        /// <summary>
        ///     value alignment
        /// </summary>
        public DerivedValueOption<Alignment> Align { get; }

        /// <summary>
        ///     Application type
        /// </summary>
        public DerivedValueOption<AppType> ApplicationType { get; }

        /// <summary>
        ///     boolean evaluation style
        /// </summary>
        public DerivedValueOption<BooleanEvaluation> BoolEval { get; }

        /// <summary>
        ///     clear all option values
        /// </summary>
        public void Clear() {
            Align.ResetToDefault();
        }
    }
}