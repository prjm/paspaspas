namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     formal parameter target
    /// </summary>
    public interface IParameterTarget {

        /// <summary>
        ///     parameter list by name
        /// </summary>
        ParameterDefinitions Parameters { get; }
    }
}
