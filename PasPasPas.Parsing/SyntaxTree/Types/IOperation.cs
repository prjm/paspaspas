using PasPasPas.Global.Runtime;

namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     base interface for an operator
    /// </summary>
    public interface IOperator {

        /// <summary>
        ///     operator kind (unique id)
        /// </summary>
        int Kind { get; }

        /// <summary>
        ///     operator name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     arity (number of operands)
        /// </summary>
        int Arity { get; }

        /// <summary>
        ///     used type registry
        /// </summary>
        ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     evaluate an operator and compute its results (if operands are constants)
        /// </summary>
        /// <param name="input">operator input - type reference or constant</param>
        /// <returns>operator result - type reference or constant</returns>
        ITypeReference EvaluateOperator(Signature input);

    }
}
