namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     result of a function
    /// </summary>
    public class FunctionResult : VariableName {

        /// <summary>
        ///     method implementation
        /// </summary>
        public MethodImplementation Method { get; set; }
    }
}
