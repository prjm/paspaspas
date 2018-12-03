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
            DebugOptions = new DebugOptions(baseOptions?.DebugOptions);
            RuntimeChecks = new RuntimeCheckOptions(baseOptions?.RuntimeChecks);
            CodeGeneration = new CodeGenerationOptions(baseOptions?.CodeGeneration);
            Syntax = new SyntaxOptions(baseOptions?.Syntax);
            AdditionalOptions = new AdditionalCompilerOptions(baseOptions?.AdditionalOptions);
            LinkOptions = new LinkOptions(baseOptions?.LinkOptions);
            HintsAndWarnings = new HintsAndWarningsOptions(baseOptions?.HintsAndWarnings);
        }


        /// <summary>
        ///     Debug-related options
        /// </summary>
        public DebugOptions DebugOptions { get; }

        /// <summary>
        ///     runtime checks
        /// </summary>
        public RuntimeCheckOptions RuntimeChecks { get; }

        /// <summary>
        ///     code generation options
        /// </summary>
        public CodeGenerationOptions CodeGeneration { get; }

        /// <summary>
        ///     syntax options
        /// </summary>
        public SyntaxOptions Syntax { get; }

        /// <summary>
        ///     additional compiler options
        /// </summary>
        public AdditionalCompilerOptions AdditionalOptions { get; }

        /// <summary>
        ///     link options
        /// </summary>
        public LinkOptions LinkOptions { get; }

        /// <summary>
        ///     hints and warnings
        /// </summary>
        public HintsAndWarningsOptions HintsAndWarnings { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        public void Clear() {
            DebugOptions.Clear();
            RuntimeChecks.Clear();
            CodeGeneration.Clear();
            Syntax.Clear();
            AdditionalOptions.Clear();
            LinkOptions.Clear();
            HintsAndWarnings.Clear();
        }

        /// <summary>
        ///     reset compile options for a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            DebugOptions.ResetOnNewUnit();
            RuntimeChecks.ResetOnNewUnit();
            CodeGeneration.ResetOnNewUnit();
            Syntax.ResetOnNewUnit();
            AdditionalOptions.ResetOnNewUnit();
            HintsAndWarnings.ResetOnNewUnit();
            LinkOptions.ResetOnNewUnit();
        }
    }
}
