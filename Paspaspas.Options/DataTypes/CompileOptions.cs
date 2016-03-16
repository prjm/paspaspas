namespace PasPasPas.Options.DataTypes {

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
            ExportCppObjects = new DerivedValueOption<ExportCppObjects>(baseOptions?.ExportCppObjects);
            ExtendedCompatibility = new DerivedValueOption<ExtendedCompatiblityMode>(baseOptions?.ExtendedCompatibility);
            UseExtendedSyntax = new DerivedValueOption<ExtendedSyntax>(baseOptions?.UseExtendedSyntax);
            ExcessPrecision = new DerivedValueOption<ExcessPrecisionForResults>(baseOptions?.ExcessPrecision);
            HighCharUnicode = new DerivedValueOption<HighCharsUnicode>(baseOptions?.HighCharUnicode);
            Hints = new DerivedValueOption<CompilerHints>(baseOptions?.Hints);
            ImageBase = new DerivedValueOption<int>(baseOptions?.ImageBase);
        }

        /// <summary>
        ///     image base address
        /// </summary>
        public DerivedValueOption<int> ImageBase { get; set; }

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
        ///     excess precision on x64
        /// </summary>
        public DerivedValueOption<ExcessPrecisionForResults> ExcessPrecision { get; }

        /// <summary>
        ///     export all cpp objects
        /// </summary>
        public DerivedValueOption<ExportCppObjects> ExportCppObjects { get; }

        /// <summary>
        ///     exteded compatibility mode
        /// </summary>
        public DerivedValueOption<ExtendedCompatiblityMode> ExtendedCompatibility { get; }

        /// <summary>
        ///     high chars for unicode
        /// </summary>
        public DerivedValueOption<HighCharsUnicode> HighCharUnicode { get; }

        /// <summary>
        ///     enable or disable hints
        /// </summary>
        public DerivedValueOption<CompilerHints> Hints { get; }

        /// <summary>
        ///     switch to enable extended syntax
        /// </summary>
        public DerivedValueOption<ExtendedSyntax> UseExtendedSyntax { get; }

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
            ExportCppObjects.ResetToDefault();
            ExtendedCompatibility.ResetToDefault();
            UseExtendedSyntax.ResetToDefault();
            ExcessPrecision.ResetToDefault();
            HighCharUnicode.ResetToDefault();
            Hints.ResetToDefault();
            ImageBase.ResetToDefault();
        }

        /// <summary>
        ///     reset compile options for a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            Align.ResetToDefault();
            BoolEval.ResetToDefault();
            CodeAlign.ResetToDefault();
            Assertions.ResetToDefault();
            ExtendedCompatibility.ResetToDefault();
            ExcessPrecision.ResetToDefault();
            HighCharUnicode.ResetToDefault();
            Hints.ResetToDefault();
        }
    }
}
