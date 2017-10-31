namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base interface for an operation
    /// </summary>
    public interface IOperation {
        /// <summary>
        ///     operation kind
        /// </summary>
        int Kind { get; }

        /// <summary>
        ///     operation name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     compute a type signature
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        int GetOutputTypeForOperation(Signature input);
    }
}
