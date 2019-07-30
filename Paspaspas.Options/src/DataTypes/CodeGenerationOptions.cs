using PasPasPas.Globals.Options;
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     code generation options
    /// </summary>
    public class CodeGenerationOptions : ICodeGenerationOptions {

        /// <summary>
        ///     create a new set of code generation options
        /// </summary>
        /// <param name="baseOptions"></param>
        public CodeGenerationOptions(ICodeGenerationOptions baseOptions) {
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
        public IOption<RttiForPublishedPropertieMode> PublishedRtti { get; }

        /// <summary>
        ///     minimum enum size
        /// </summary>
        public IOption<EnumSize> MinimumEnumSize { get; }

        /// <summary>
        ///     flag to enable optimization
        /// </summary>
        public IOption<CompilerOptimization> Optimization { get; }

        /// <summary>
        ///     save divide option
        /// </summary>
        public IOption<FDivSafeDivide> SafeDivide { get; }

        /// <summary>
        ///     value alignment
        /// </summary>
        public IOption<Alignment> Align { get; }

        /// <summary>
        ///     generate all stack frames
        /// </summary>
        public IOption<StackFrameGeneration> StackFrames { get; }

        /// <summary>
        ///     code alignment
        /// </summary>
        public IOption<CodeAlignment> CodeAlign { get; }

        /// <summary>
        ///     rtti options
        /// </summary>
        public IRttiOptions Rtti { get; }

        /// <summary>
        ///     enable or disable method info generation
        /// </summary>
        public IOption<MethodInfoRttiMode> MethodInfo { get; }

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
