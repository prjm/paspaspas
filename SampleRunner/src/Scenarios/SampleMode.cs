#nullable disable
namespace SampleRunner.Scenarios {

    /// <summary>
    ///     sample mode
    /// </summary>
    public enum SampleMode {

        /// <summary>
        ///     undefined mode
        /// </summary>
        Undefined,

        /// <summary>
        ///     read a file
        /// </summary>
        ReadFile,

        /// <summary>
        ///     tokenizer a file
        /// </summary>
        TokenizerFile,

        /// <summary>
        ///     tokenize a file in a buffer
        /// </summary>
        BufferedTokenizeFile,

        /// <summary>
        ///     parse a file
        /// </summary>
        ParseFile,

        /// <summary>
        ///     parse a file, create an abstract syntax tree
        /// </summary>
        CreateAbstractSyntaxTree,

        /// <summary>
        ///     parse a file, create an abstract syntax tree, add type annotations
        /// </summary>
        TypeAnnotateFile,

        /// <summary>
        ///     find constant values
        /// </summary>
        FindConstants,

        /// <summary>
        ///     create an assembly
        /// </summary>
        CreateAssembly,
    }
}
