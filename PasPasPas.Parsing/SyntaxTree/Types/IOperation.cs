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
        ///     compute a type signature
        /// </summary>
        /// <param name="input">operator input</param>
        /// <returns></returns>
        ITypeReference EvaluateOperator(Signature input);

        /// <summary>
        ///     compute the value of this operator
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        IValue ComputeValue(IValue[] inputs);

    }
}
