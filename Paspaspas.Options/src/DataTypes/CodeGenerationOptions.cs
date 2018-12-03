namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     code generation options
    /// </summary>
    public class CodeGenerationOptions {

        /// <summary>
        ///     create a new set of code generation options
        /// </summary>
        /// <param name="baseOptions"></param>
        public CodeGenerationOptions(CodeGenerationOptions baseOptions) {
            PublishedRtti = new DerivedValueOption<RttiForPublishedPropertieMode>(baseOptions?.PublishedRtti);
            MinimumEnumSize = new DerivedValueOption<EnumSize>(baseOptions?.MinimumEnumSize);
            Optimization = new DerivedValueOption<CompilerOptimization>(baseOptions?.Optimization);
            SafeDivide = new DerivedValueOption<FDivSafeDivide>(baseOptions?.SafeDivide);
            Align = new DerivedValueOption<Alignment>(baseOptions?.Align);
            StackFrames = new DerivedValueOption<StackFrameGeneration>(baseOptions?.StackFrames);
            CodeAlign = new DerivedValueOption<CodeAlignment>(baseOptions?.CodeAlign);
            Rtti = new RttiOptions(baseOptions?.Rtti);
            MethodInfo = new DerivedValueOption<MethodInfoRttiMode>(baseOptions?.MethodInfo);
        }

        /// <summary>
        ///     flag to generate rtti for published fields
        /// </summary>
        public DerivedValueOption<RttiForPublishedPropertieMode> PublishedRtti { get; }

        /// <summary>
        ///     minimum enum size
        /// </summary>
        public DerivedValueOption<EnumSize> MinimumEnumSize { get; }

        /// <summary>
        ///     flag to enable optimization
        /// </summary>
        public DerivedValueOption<CompilerOptimization> Optimization { get; }

        /// <summary>
        ///     save divide option
        /// </summary>
        public DerivedValueOption<FDivSafeDivide> SafeDivide { get; }

        /// <summary>
        ///     value alignment
        /// </summary>
        public DerivedValueOption<Alignment> Align { get; }

        /// <summary>
        ///     generate all stack frames
        /// </summary>
        public DerivedValueOption<StackFrameGeneration> StackFrames { get; }

        /// <summary>
        ///     code alignment
        /// </summary>
        public DerivedValueOption<CodeAlignment> CodeAlign { get; }

        /// <summary>
        ///     rtti options
        /// </summary>
        public RttiOptions Rtti { get; }

        /// <summary>
        ///     enable or disable method info generation
        /// </summary>
        public DerivedValueOption<MethodInfoRttiMode> MethodInfo { get; }

        /// <summary>
        ///     clear values
        /// </summary>
        public void Clear() {
            PublishedRtti.ResetToDefault();
            MinimumEnumSize.ResetToDefault();
            Optimization.ResetToDefault();
            SafeDivide.ResetToDefault();
            Align.ResetToDefault();
            StackFrames.ResetToDefault();
            CodeAlign.ResetToDefault();
            Rtti.ResetToDefault();
            MethodInfo.ResetToDefault();
        }

        /// <summary>
        ///     reset options
        /// </summary>
        public void ResetOnNewUnit() {
            PublishedRtti.ResetToDefault();
            MinimumEnumSize.ResetToDefault();
            Optimization.ResetToDefault();
            SafeDivide.ResetToDefault();
            Align.ResetToDefault();
            StackFrames.ResetToDefault();
            CodeAlign.ResetToDefault();
            Rtti.ResetToDefault();
            MethodInfo.ResetToDefault();
        }


    }
}
