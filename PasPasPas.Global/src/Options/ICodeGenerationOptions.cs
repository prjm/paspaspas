using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     code generation options
    /// </summary>
    public interface ICodeGenerationOptions {

        /// <summary>
        ///     code alignment options
        /// </summary>
        IOption<CodeAlignment> CodeAlign { get; }

        /// <summary>
        ///     data alignment options
        /// </summary>
        IOption<Alignment> Align { get; }

        /// <summary>
        ///     optimization mode
        /// </summary>
        IOption<CompilerOptimization> Optimization { get; }

        /// <summary>
        ///     save divide
        /// </summary>
        IOption<FDivSafeDivide> SafeDivide { get; }

        /// <summary>
        ///     stack frame options
        /// </summary>
        IOption<StackFrameGeneration> StackFrames { get; }

        /// <summary>
        ///     minimum enum size
        /// </summary>
        IOption<EnumSize> MinimumEnumSize { get; }

        /// <summary>
        ///     published rtti information
        /// </summary>
        IOption<RttiForPublishedPropertieMode> PublishedRtti { get; }

        /// <summary>
        ///     rtti options
        /// </summary>
        IRttiOptions Rtti { get; }

        /// <summary>
        ///     rtti method info generation
        /// </summary>
        IOption<MethodInfoRttiMode> MethodInfo { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();

        /// <summary>
        ///     reset option on a new unit
        /// </summary>
        void ResetOnNewUnit();
    }
}