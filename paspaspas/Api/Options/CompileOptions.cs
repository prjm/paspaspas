namespace PasPasPas.Api.Options {

    /// <summary>
    ///     compile-specific options
    /// </summary>
    public class CompileOptions {

        /// <summary>
        ///     compiler-specific options
        /// </summary>
        /// <param name="baseOptions"></param>
        public CompileOptions(CompileOptions baseOptions) {
            Align = new DerivedValueOption<Alignment>(baseOptions?.Align);
            ApplicationType = new DerivedValueOption<AppType>(baseOptions?.ApplicationType);
            BoolEval = new DerivedValueOption<BooleanEvaluation>(baseOptions?.BoolEval);
            CodeAlign = new DerivedValueOption<CodeAlignment>(baseOptions?.CodeAlign);
            Assertions = new DerivedValueOption<AssertionMode>(baseOptions?.Assertions);
            DebugInfo = new DerivedValueOption<DebugInformation>(baseOptions?.DebugInfo);
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
        ///     Assertion mode
        /// </summary>
        public DerivedValueOption<AssertionMode> Assertions { get; }

        /// <summary>
        ///     boolean evaluation style
        /// </summary>
        public DerivedValueOption<BooleanEvaluation> BoolEval { get; }

        /// <summary>
        ///     code alignment
        /// </summary>
        public DerivedValueOption<CodeAlignment> CodeAlign { get; }

        /// <summary>
        ///     debug info
        /// </summary>
        public DerivedValueOption<DebugInformation> DebugInfo { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        public void Clear() {
            Align.ResetToDefault();
            ApplicationType.ResetToDefault();
            BoolEval.ResetToDefault();
            CodeAlign.ResetToDefault();
            Assertions.ResetToDefault();
            DebugInfo.ResetToDefault();
        }

        /// <summary>
        ///     reset compile options for a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            Align.ResetToDefault();
            BoolEval.ResetToDefault();
            CodeAlign.ResetToDefault();
            Assertions.ResetToDefault();
        }
    }
}
