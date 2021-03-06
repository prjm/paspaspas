﻿#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     base interface for an operator
    /// </summary>
    public interface IOperator {

        /// <summary>
        ///     operator kind (unique id)
        /// </summary>
        OperatorKind Kind { get; }

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
        ///     evaluate an operator and compute its results
        /// </summary>
        /// <param name="input">operator input - type reference or constant</param>
        /// <param name="currentUnit">current unit</param>
        /// <returns>operator result - type reference or constant</returns>
        ITypeSymbol EvaluateOperator(ISignature input, IUnitType currentUnit);

    }
}
