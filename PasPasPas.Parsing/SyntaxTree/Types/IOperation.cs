using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     base interface for an operation
    /// </summary>
    public interface IOperator {
        /// <summary>
        ///     operation kind
        /// </summary>
        int Kind { get; }

        /// <summary>
        ///     operation name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     used type registry
        /// </summary>
        ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     compute a type signature
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        int GetOutputTypeForOperation(Signature input);
    }
}
